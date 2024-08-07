using BepInEx;
using HarmonyLib;
using AtlasLib.Pages;
using AtlasLib.Saving;

namespace AtlasLib;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "wafflethings.atlaslib";
    private const string Name = "AtlasLib";
    private const string Version = "3.0.1";

    private void Start()
    {
        PageRegistry.Initialize();
        new Harmony(Guid).PatchAll();
    }

    private void OnDestroy()
    {
        SaveFile.SaveAll();
    }
}
