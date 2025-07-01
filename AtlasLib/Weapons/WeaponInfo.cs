using AtlasLib.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace AtlasLib.Weapons;

[CreateAssetMenu(menuName = "AtlasLib/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    public GameObject[] WeaponObjects = new GameObject[1];
    public WeaponType WeaponType = WeaponType.Gun;
    [Space(10)]
    public string Id = "rev0";
    public int Slot = 0;
    [FormerlySerializedAs("OrderInSlot")] public int IndexInSlot;
    public bool UseFreshness = true;
        
    public WeaponInfo(GameObject[] weaponObjects, string id, int slot, WeaponType weaponType = WeaponType.Gun, int indexInSlot = 0, bool useFreshness = true)
    {
        WeaponObjects = weaponObjects;
        Id = id;
        Slot = slot;
        WeaponType = weaponType;
        IndexInSlot = indexInSlot;
        UseFreshness = useFreshness;
    }
}
