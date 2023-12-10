using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AtlasLib.Weapons;
using AtlasLib.Utils;
using UnityEngine;

namespace AtlasLib.Weapons
{
    public static class WeaponRegistry
    {
        private static string CurrentSave => string.Format(SavePath, GameProgressSaver.currentSlot);
        private static string SavePath = Path.Combine(PathUtils.ModPath(), "save{0}");
        private static Dictionary<string, int> WeaponOwnership = new();
        public static List<Weapon> Weapons = new();
        public static List<Weapon> Guns = new();
        public static List<Weapon> Fists = new();

        public static void Initialize()
        {
            Plugin.Harmony.PatchAll(typeof(WeaponRegistry));
            LoadData();
        }

        public static void LoadData()
        {
            WeaponOwnership.Clear();

            if (File.Exists(CurrentSave))
            {
                foreach (string line in File.ReadLines(CurrentSave))
                {
                    string[] data = line.Split('~');
                    WeaponOwnership.Add(data[0], int.Parse(data[1]));
                }
            }
        }

        public static void SaveData()
        {
            using (StreamWriter sw = File.CreateText(CurrentSave))
            {
                foreach (KeyValuePair<string, int> ownershipPair in WeaponOwnership)
                {
                    sw.WriteLine($"{ownershipPair.Key}~{ownershipPair.Value}");
                }
            }
        }

        public static bool CheckOwnership(string id)
        {
            if (!WeaponOwnership.ContainsKey(id))
            {
                WeaponOwnership.Add(id, 0);
            }

            return WeaponOwnership[id] == 1;
        }

        public static void Register(Weapon weapon)
        {
            if (weapon.Info.WeaponType == WeaponType.Gun)
            {
                Guns.Add(weapon);
            }
            else
            {
                Fists.Add(weapon);
            }

            Weapons.Add(weapon);
        }

        [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.SetSlot))]
        [HarmonyPrefix]
        public static void SaveOnSlotChange()
        {
            SaveData();
        }

        [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.SetSlot))]
        [HarmonyPostfix]
        public static void LoadOnSlotChange()
        {
            LoadData();
        }

        [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))]
        [HarmonyPostfix]
        public static void GiveGuns(GunSetter __instance)
        {
            foreach (Weapon weapon in Guns)
            {
                if (weapon.Selection != WeaponSelection.Disabled)
                {
                    GameObject created = weapon.Create(__instance.transform);
                    __instance.gunc.slots[weapon.Info.Slot].Add(created);
                    created.SetActive(false);
                }
            }
        }

        [HarmonyPatch(typeof(FistControl), nameof(FistControl.ResetFists))]
        [HarmonyPostfix]
        public static void GiveFists(FistControl __instance)
        {
            foreach (Weapon weapon in Fists.OrderBy(fist => fist.Info.Slot))
            {
                if (weapon.Selection != WeaponSelection.Disabled && weapon.Owned)
                {
                    GameObject created = weapon.Create(__instance.transform);
                    created.SetActive(false);

                    FistControl.Instance.spawnedArms.Add(created);
                    FistControl.Instance.spawnedArmNums.Add(weapon.Info.Slot);
                }
            }
        }

        [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.CheckGear))]
        [HarmonyPrefix]
        public static bool CheckGearForCustoms(ref int __result, string gear)
        {
            foreach (Weapon weapon in Weapons)
            {
                if (weapon.Info.Id == gear)
                {
                    __result = WeaponOwnership["weapon." + weapon.Info.Id];
                    return false;
                }
            }

            return true;
        }

        [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.AddGear))]
        [HarmonyPrefix]
        public static bool AddGearForCustoms(string gear)
        {
            foreach (Weapon weapon in Weapons)
            {
                if (weapon.Info.Id == gear)
                {
                    WeaponOwnership["weapon." + weapon.Info.Id] = 1;
                    return false;
                }
            }

            return true;
        }
    }
}
