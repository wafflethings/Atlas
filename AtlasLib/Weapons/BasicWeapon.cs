namespace AtlasLib.Weapons;

public class BasicWeapon : Weapon
{
    private WeaponInfo _info;

    public BasicWeapon(WeaponInfo info)
    {
            _info = info;
    }

    public override WeaponInfo Info => _info;
}