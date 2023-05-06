using Atlas.Modules;
using Atlas.Modules.Guns;
using Atlas.Modules.Terminal;
using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atlas
{
    [BepInPlugin("waffle.ultrakill.atlas", "Atlas", "1.0.0")]
    public class Atlas : BaseUnityPlugin
    {
        public static AssetBundle Assets;

        private static Harmony Harmony = new Harmony("waffle.ultrakill.atlas");
        public Module[] Modules =
        {
            new TerminalPageRegistry(),
            new GunRegistry()
        };

        public void Start()
        {
            foreach(Module module in Modules)
            {
                module.Patch(Harmony);
            }
            /*
            Assets = AssetBundle.LoadFromFile(Path.Combine(PathUtils.ModDirectory(), "Atlas", "atlastest.bundle"));

            TerminalPageRegistry.RegisterPage(typeof(TemplatePage));

            Test.Asset = Assets.LoadAsset<GameObject>("Template Gun.prefab");
            TestGreen.Asset = Assets.LoadAsset<GameObject>("Template Gun 1.prefab");
            GunRegistry.Register(typeof(Test));
            GunRegistry.Register(typeof(TestGreen));
            */
        }

        public void OnDestroy()
        {
            GunRegistry.SaveData();
        }
    }
}
