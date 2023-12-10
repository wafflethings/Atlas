using AtlasLib.Weapons;
using UnityEngine;

namespace AtlasLib.Weapons
{
    [CreateAssetMenu(menuName = "AtlasLib/Weapon Info")]
    public class WeaponInfo : ScriptableObject
    {
        public GameObject[] WeaponObjects = new GameObject[1];
        public string Id = "rev0";
        public int Slot = 0;
        public WeaponType WeaponType = WeaponType.Gun;
        public bool UseFreshness = true;
        
        public WeaponInfo(GameObject[] weaponObjects, string id, int slot, WeaponType weaponType = WeaponType.Gun, bool useFreshness = true)
        {
            WeaponObjects = weaponObjects;
            Id = id;
            Slot = slot;
            WeaponType = weaponType;
            UseFreshness = useFreshness;
        }
    }
}