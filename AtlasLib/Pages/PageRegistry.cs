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
    private static int s_currentPage;
    private static readonly List<Page> s_pages = new() { new DefaultWeaponPage() };
    private static GameObject s_leftScrollButton;
    private static GameObject s_rightScrollButton;

    public static void RegisterPage(Page page, int at = -1)
    {
        if (at == -1)
        {
            s_pages.Add(page);
        }
        else
        {
            s_pages.Insert(at, page);
        }
    }
    
    public static void RegisterPages(IEnumerable<Page> pages)
    {
        foreach (Page page in pages)
        {
            RegisterPage(page);
        }
    }

    public static void RefreshPages(int changedBy)
    {
        if (s_pages.Count == 0)
        {
            s_leftScrollButton.SetActive(false);
            s_leftScrollButton.SetActive(true);
            return;
        }
            
        s_pages[s_currentPage].EnablePage();

        if (changedBy != 0)
        {
            s_pages[s_currentPage - changedBy].DisablePage();
        }
    }

    public static void ButtonScroll(bool isLeft)
    {
        if (isLeft)
        {
            if (s_currentPage > 0)
            {
                s_currentPage -= 1;
                RefreshPages(-1);
            }
        }
        else
        {
            if (s_currentPage < s_pages.Count - 1)
            {
                s_currentPage += 1;
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

            s_currentPage = 0;

            foreach (Page page in s_pages)
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
