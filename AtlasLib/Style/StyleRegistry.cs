using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace AtlasLib.Style;

[HarmonyPatch]
public static class StyleRegistry
{
    private static List<Style>s_styles = new();
        
    public static void Register(Style style)
    {
        s_styles.Add(style);
    }

    [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start)), HarmonyPostfix]
    private static void AddCustomStyles(StyleHUD __instance)
    {
        foreach (Style style in s_styles)
        {
            if (style.FreshnessDecayMultiplier != 1)
            {
                __instance.freshnessDecayMultiplierDict.Add(style.Id, style.FreshnessDecayMultiplier);
            }
                
            __instance.RegisterStyleItem(style.Id, style.FullString);
        }
    }
}
