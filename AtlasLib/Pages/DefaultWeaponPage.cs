using AtlasLib.Utils;
using UnityEngine;

namespace AtlasLib.Pages;

internal class DefaultWeaponPage : Page
{
    public override void CreatePage(Transform parent)
    {
        base.CreatePage(parent);
        Objects.Add(parent.Find("Weapons Panel").gameObject);
    }
}
