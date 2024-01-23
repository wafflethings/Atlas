using AtlasLib.Weapons;
using UnityEngine;

namespace AtlasLib.Weapons
{
    [CreateAssetMenu(menuName = "AtlasLib/Weapon Info")]
    public class WeaponInfo : ScriptableObject
    {
        public GameObject[] WeaponObjects = new GameObject[1];
        public WeaponType WeaponType = WeaponType.Gun;
        [Space(10)]
        public string Id = "rev0";
        public int Slot = 0;
        public int OrderInSlot;
        public bool UseFreshness = true;
        
        public WeaponInfo(GameObject[] weaponObjects, string id, int slot, WeaponType weaponType = WeaponType.Gun, int orderInSlot = 0, bool useFreshness = true)
        {
            WeaponObjects = weaponObjects;
            Id = id;
            Slot = slot;
            WeaponType = weaponType;
            OrderInSlot = orderInSlot;
            UseFreshness = useFreshness;
        }
    }
}