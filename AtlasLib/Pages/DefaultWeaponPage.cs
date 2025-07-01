using AtlasLib.Utils;
using UnityEngine;

namespace AtlasLib.Pages;

internal class DefaultWeaponPage : Page
{
    private static readonly string[] s_page1Content =
    {
        "Weapons Panel/Menu Title",
        
        "Weapons Panel/Buttons/RevolverButton",
        "Weapons Panel/Buttons/ShotgunButton",
        "Weapons Panel/Buttons/NailgunButton",
        "Weapons Panel/Buttons/RailcannonButton",
        "Weapons Panel/Buttons/RocketLauncherButton",
        "Weapons Panel/Buttons/ArmButton",
        // but NOT the back button

        "Revolver Window",
        "Shotgun Window",
        "Nailgun Window",
        "Railcannon Window",
        "Rocket Launcher Window",
        "Arm Window"
    };

    public override void CreatePage(Transform parent)
    {
        base.CreatePage(parent);

        foreach (string baseObject in s_page1Content)
        {
            Objects.Add(parent.gameObject.transform.Find(baseObject).gameObject);
        }

        PageButtonHolder = parent.gameObject.transform.Find("Weapons Panel/Buttons/");
    }
}
