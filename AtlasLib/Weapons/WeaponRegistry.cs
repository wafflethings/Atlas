using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AtlasLib.Saving;
using AtlasLib.Weapons;
using AtlasLib.Utils;
using UnityEngine;

namespace AtlasLib.Weapons;

[HarmonyPatch]
public static class WeaponRegistry
{
    public static readonly List<Weapon> Weapons = new();
    private static readonly SaveFile<Dictionary<int, Dictionary<string, int>>> s_weaponOwnership = SaveFile.RegisterFile(new SaveFile<Dictionary<int, Dictionary<string, int>>>("weapons_owned.json", default));
    private static List<Weapon> s_guns = new();
    private static List<Weapon> s_fists = new();
    
    public static bool CheckOwnership(string id)
    {
        if (id.StartsWith("weapon."))
        {
            throw new Exception("Id starts with 'weapon.'. Don't do that, it gets added automatically");
        }

        id = "weapon." + id;
            
        if (!CurrentOwnershipDict.ContainsKey(id))
        {
            CurrentOwnershipDict.Add(id, 0);
        }

        return CurrentOwnershipDict[id] == 1;
    }

    public static void RegisterWeapon(Weapon weapon)
    {
        if (weapon.Info.WeaponType == WeaponType.Gun)
        {
            s_guns.Add(weapon);
        }
        else
        {
            s_fists.Add(weapon);
        }

        Weapons.Add(weapon);
    }

    public static void RegisterWeapons(IEnumerable<Weapon> weapons)
    {
        foreach (Weapon weapon in weapons)
        {
            RegisterWeapon(weapon);
        }
    }

    private static void EnsureDictExistsForSlot()
    {
        if (s_weaponOwnership.Data.ContainsKey(GameProgressSaver.currentSlot))
        {
            return;
        }
        
        s_weaponOwnership.Data.Add(GameProgressSaver.currentSlot, new());
    }

    private static Dictionary<string, int> CurrentOwnershipDict
    {
        get
        {
            EnsureDictExistsForSlot();
            return s_weaponOwnership.Data[GameProgressSaver.currentSlot];
        }
    }

    [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))]
    [HarmonyPostfix]
    private static void GiveGuns(GunSetter __instance)
    {
        s_guns = s_guns.OrderBy(gun => gun.Info.OrderInSlot).ToList();
        foreach (Weapon weapon in s_guns)
        {
            if (weapon.Selection == WeaponSelection.Disabled)
            {
                continue;
            }
            
            // i would LOVE to use GunControl.slots, but gunsetter.resetweapons is called in start, before gc.start.
            // this means that slots isnt set before this runs initially.

            List<List<GameObject>> slots = new()
            {
                __instance.gunc.slot1,
                __instance.gunc.slot2,
                __instance.gunc.slot3,
                __instance.gunc.slot4,
                __instance.gunc.slot5,
                __instance.gunc.slot6
            };

            GameObject created = weapon.Create(__instance.transform);
            slots[weapon.Info.Slot].Add(created);
            created.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(FistControl), nameof(FistControl.ResetFists))]
    [HarmonyPostfix]
    private static void GiveFists(FistControl __instance)
    {
        s_fists = s_fists.OrderBy(gun => gun.Info.OrderInSlot).ToList();
        foreach (Weapon weapon in s_fists)
        {
            if (weapon.Selection == WeaponSelection.Disabled || !weapon.Owned)
            {
                return;
            }

            GameObject created = weapon.Create(__instance.transform);
            created.SetActive(false);

            FistControl.Instance.spawnedArms.Add(created);
            FistControl.Instance.spawnedArmNums.Add(weapon.Info.Slot);
        }
    }

    [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.CheckGear))]
    [HarmonyPrefix]
    private static bool CheckGearForCustoms(ref int __result, string gear)
    {
        foreach (Weapon weapon in Weapons)
        {
            if (weapon.Info?.Id != gear)
            {
                continue;
            }
            
            __result = CurrentOwnershipDict["weapon." + weapon.Info.Id];
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.AddGear))]
    [HarmonyPrefix]
    private static bool AddGearForCustoms(string gear)
    {
        foreach (Weapon weapon in Weapons)
        {
            if (weapon.Info.Id != gear)
            {
                continue;
            }

            CurrentOwnershipDict["weapon." + weapon.Info.Id] = 1;
            return false;
        }
        
        return true;
    }
}
