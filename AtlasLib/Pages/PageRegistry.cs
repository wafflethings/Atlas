using System;
using System.Collections.Generic;
using AtlasLib.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AtlasLib.Pages
{
    public static class PageRegistry
    {
        public static int CurrentPage { get; private set; }
        public static List<Page> Pages { get; private set; } = new();

        public static void Initialize()
        {
            Pages.Add(new DefaultWeaponPage());
            Plugin.Harmony.PatchAll(typeof(PageRegistry));
        }

        public static void Register(Page page, int at = -1)
        {
            if (at == -1)
            {
                Pages.Add(page);
            }
            else
            {
                Pages.Insert(at, page);
            }
        }

        public static void RefreshPages(int changedBy)
        {
            Pages[CurrentPage].EnablePage();

            if (changedBy != 0)
            {
                Pages[CurrentPage - changedBy].DisablePage();
            }
        }

        public static void ButtonScroll(bool isLeft)
        {
            if (isLeft)
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    RefreshPages(-1);
                }
            }
            else
            {
                if (CurrentPage < Pages.Count - 1)
                {
                    CurrentPage += 1;
                    RefreshPages(1);
                }
            }
        }

        [HarmonyPatch(typeof(ShopZone), nameof(ShopZone.Start))]
        [HarmonyPostfix]
        public static void ShopStart(ShopZone __instance)
        {
            if (__instance.gameObject.GetChild("Canvas")?.GetChild("Weapons") != null)
            {
                // If this is true, it's a yellow shop (e.g: not a testament)

                GameObject templateButton = __instance.gameObject.GetChild("Canvas/Weapons/BackButton (1)");
                ShopButton templateShopButton = templateButton.GetComponent<ShopButton>();
                templateShopButton.toActivate = Array.Empty<GameObject>();
                templateShopButton.toDeactivate = Array.Empty<GameObject>();

                GameObject leftScrollButton = Object.Instantiate(templateButton, templateButton.transform.parent);
                RectTransform leftRect = leftScrollButton.GetComponent<RectTransform>();
                leftRect.sizeDelta -= new Vector2(leftRect.sizeDelta.x / 2, 0);
                leftScrollButton.transform.localPosition = new Vector3(-220, -145, -45);

                GameObject rightScrollButton = Object.Instantiate(leftScrollButton, templateButton.transform.parent);
                rightScrollButton.transform.localPosition = new Vector3(-140, -145, -45);

                leftScrollButton.GetComponentInChildren<Text>().text = "<<";
                leftScrollButton.GetComponent<RectTransform>().SetAsFirstSibling();
                leftScrollButton.GetComponentInChildren<Button>().onClick.AddListener(() => ButtonScroll(true));

                rightScrollButton.GetComponentInChildren<Text>().text = ">>";
                rightScrollButton.GetComponent<RectTransform>().SetAsFirstSibling();
                rightScrollButton.GetComponentInChildren<Button>().onClick.AddListener(() => ButtonScroll(false));

                CurrentPage = 0;

                foreach (Page page in Pages)
                {
                    page.CreatePage(__instance.gameObject.GetChild("Canvas/Weapons").transform);

                    if (page.GetType() != typeof(DefaultWeaponPage))
                    {
                        page.DisablePage();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShopZone), nameof(ShopZone.TurnOn))]
        [HarmonyPostfix]
        public static void RefreshOnEntrance()
        {
            RefreshPages(0);
        }
    }
}
