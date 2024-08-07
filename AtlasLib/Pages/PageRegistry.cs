using System;
using System.Collections.Generic;
using AtlasLib.Utils;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AtlasLib.Pages;

[HarmonyPatch]
public static class PageRegistry
{
    public static int CurrentPage { get; private set; }
    public static List<Page> Pages { get; } = new();

    private static GameObject s_leftScrollButton;
    private static GameObject s_rightScrollButton;
        
    internal static void Initialize()
    {
        Pages.Add(new DefaultWeaponPage());
    }

    public static void RegisterPage(Page page, int at = -1)
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

    public static void RegisterPages(Page[] pages)
    {
        foreach (Page page in pages)
        {
            RegisterPage(page);
        }
    }

    public static void RefreshPages(int changedBy)
    {
        if (Pages.Count == 0)
        {
            s_leftScrollButton.SetActive(false);
            s_leftScrollButton.SetActive(true);
            return;
        }
            
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
    private static void ShopStart(ShopZone __instance)
    {
        if (__instance.gameObject.GetChild("Canvas")?.GetChild("Weapons") != null)
        {
            // If this is true, it's a yellow shop (e.g: not a testament)

            GameObject backButton = __instance.gameObject.GetChild("Canvas/Weapons/BackButton (1)");
            GameObject templateButton = Object.Instantiate(backButton, backButton.transform.parent);
            ShopButton templateShopButton = templateButton.GetComponent<ShopButton>();
            templateShopButton.toActivate = Array.Empty<GameObject>();
            templateShopButton.toDeactivate = Array.Empty<GameObject>();

            s_leftScrollButton = Object.Instantiate(templateButton, templateButton.transform.parent);
            RectTransform leftRect = s_leftScrollButton.GetComponent<RectTransform>();
            leftRect.sizeDelta -= new Vector2(leftRect.sizeDelta.x / 2, 0);
            s_leftScrollButton.transform.localPosition = new Vector3(-220, -145, -45);

            s_rightScrollButton = Object.Instantiate(s_leftScrollButton, templateButton.transform.parent);
            s_rightScrollButton.transform.localPosition = new Vector3(-140, -145, -45);

            s_leftScrollButton.GetComponentInChildren<TMP_Text>().text = "<<";
            s_leftScrollButton.GetComponent<RectTransform>().SetAsFirstSibling();
            s_leftScrollButton.GetComponentInChildren<Button>().onClick.AddListener(() => ButtonScroll(true));

            s_rightScrollButton.GetComponentInChildren<TMP_Text>().text = ">>";
            s_rightScrollButton.GetComponent<RectTransform>().SetAsFirstSibling();
            s_rightScrollButton.GetComponentInChildren<Button>().onClick.AddListener(() => ButtonScroll(false));

            CurrentPage = 0;

            foreach (Page page in Pages)
            {
                page.CreatePage(__instance.gameObject.GetChild("Canvas/Weapons").transform);

                if (page.GetType() != typeof(DefaultWeaponPage))
                {
                    page.DisablePage();
                }
            }
                
            Object.Destroy(templateButton);
        }
    }

    [HarmonyPatch(typeof(ShopZone), nameof(ShopZone.TurnOn))]
    [HarmonyPostfix]
    private static void RefreshOnEntrance()
    {
        RefreshPages(0);
    }
}
