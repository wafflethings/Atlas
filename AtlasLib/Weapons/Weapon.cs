using UnityEngine;

namespace AtlasLib.Weapons;

public abstract class Weapon
{
    public virtual GameObject Create(Transform parent)
    {
        GameObject weapon = Object.Instantiate(Info.WeaponObjects[(int)Selection - 1], parent);

        if (Info.UseFreshness)
        {
            StyleHUD.Instance.weaponFreshness.Add(weapon, 10);
        }

        return weapon;
    }

    public abstract WeaponInfo Info { get; }

    public virtual WeaponSelection Selection => Owned ? (WeaponSelection)PrefsManager.Instance.GetInt("weapon." + Info.Id) : WeaponSelection.Disabled;
    public virtual bool Owned => WeaponRegistry.CheckOwnership(Info.Id);
}
