using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace AtlasLib.Style
{
    public static class StyleRegistry
    {
        private static List<Style> _styles = new();

        public static void Initialize()
        {
            Plugin.Harmony.PatchAll(typeof(StyleRegistry));
        }
        
        public static void Register(Style style)
        {
            _styles.Add(style);
        }

        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start)), HarmonyPostfix]
        private static void AddCustomStyles(StyleHUD __instance)
        {
            foreach (Style style in _styles)
            {
                if (style.FreshnessDecayMultiplier != 1)
                {
                    __instance.freshnessDecayMultiplierDict.Add(style.Id, style.FreshnessDecayMultiplier);
                }
                
                __instance.RegisterStyleItem(style.Id, style.FullString);
            }
        }
    }
}