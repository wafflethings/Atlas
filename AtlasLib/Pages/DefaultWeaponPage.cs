using AtlasLib.Utils;
using UnityEngine;

namespace AtlasLib.Pages
{
    internal class DefaultWeaponPage : Page
    {
        private static string[] _page1Content =
        {
            "RevolverButton",
            "ShotgunButton",
            "NailgunButton",
            "RailcannonButton",
            "RocketLauncherButton",
            "ArmButton",

            "RevolverWindow",
            "ShotgunWindow",
            "NailgunWindow",
            "RailcannonWindow",
            "RocketLauncherWindow",
            "ArmWindow"
        };

        public override void CreatePage(Transform parent)
        {
            base.CreatePage(parent);

            foreach (string baseObject in _page1Content)
            {
                Objects.Add(parent.gameObject.GetChild(baseObject));
            }
        }
    }
}