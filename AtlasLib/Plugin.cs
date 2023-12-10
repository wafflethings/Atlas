using BepInEx;
using HarmonyLib;
using AtlasLib.Pages;
using AtlasLib.Weapons;

namespace AtlasLib
{
    [BepInPlugin(GUID, Name, Version)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "waffle.ultrakill.atlas";
        public const string Name = "AtlasLib";
        public const string Version = "2.0.0";
        public static readonly Harmony Harmony = new(GUID);

        public void Start()
        {
            PageRegistry.Initialize();
            WeaponRegistry.Initialize();
        }

        public void OnDestroy()
        {
            WeaponRegistry.SaveData();
        }
    }
}