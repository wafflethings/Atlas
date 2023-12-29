using System;
using BepInEx;
using HarmonyLib;
using AtlasLib.Pages;
using AtlasLib.Style;
using AtlasLib.Utils;
using AtlasLib.Weapons;
using UnityEngine.SceneManagement;

namespace AtlasLib
{
    [BepInPlugin(GUID, Name, Version)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "waffle.ultrakill.atlas";
        public const string Name = "AtlasLib";
        public const string Version = "2.0.0";
        public static readonly Harmony Harmony = new(GUID);

        private void Start()
        {
            PageRegistry.Initialize();
            WeaponRegistry.Initialize();
            StyleRegistry.Initialize();

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (PatchThis.HasntPatched)
                {
                    PatchThis.PatchAll();
                }
            };
        }

        private void OnDestroy()
        {
            WeaponRegistry.SaveData();
        }
    }
}