using Atlas.Modules.Terminal;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atlas.Modules.Guns
{
    public class GunRegistry : Module
    {
        private static string SavePath = Path.Combine(PathUtils.ModPath(), "save");
        public static List<Weapon> WeaponList = new List<Weapon>();
        internal static Dictionary<string, int> WeaponOwned = new Dictionary<string, int>();

        private static List<List<GameObject>> SlotToList;

        public static void LoadData()
        {
            if (File.Exists(SavePath + GameProgressSaver.currentSlot)) 
            {
                List<string> silly = File.ReadLines(SavePath + GameProgressSaver.currentSlot).ToList();
                foreach (string line in silly)
                {
                    string[] stuff = line.Split('~');
                    if (WeaponOwned.ContainsKey(stuff[0]))
                    {
                        WeaponOwned[stuff[0]] = Convert.ToInt32(stuff[1]);
                    }
                    else
                    {
                        WeaponOwned.Add(stuff[0], Convert.ToInt32(stuff[1]));
                    }
                }
            }
        }

        public static void SaveData()
        {
            List<string> data = new List<string>();
             
            foreach (var kvp in WeaponOwned)
            {
                string toAdd = $"{kvp.Key}~{kvp.Value}";
                data.Add(toAdd);
            }
            File.WriteAllLines(SavePath + GameProgressSaver.currentSlot, data);
        }

        public static void Register(Weapon weapon)
        {
            WeaponList.Add(weapon);
            if (!WeaponOwned.ContainsKey("weapon." + weapon.Pref()))
            {
                WeaponOwned.Add("weapon." + weapon.Pref(), 0);
            }
        }

        public override void Patch(Harmony harmony)
        {
            harmony.PatchAll(typeof(GunRegPatches));
        }

        public class GunRegPatches
        {
            [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.SetSlot))]
            [HarmonyPrefix]
            public static void SaveOnSlotChange()
            {
                SaveData();
                string[] keys = WeaponOwned.Keys.ToArray();

                foreach (var thing in keys)
                {
                    WeaponOwned[thing] = 0;
                }
            }

            [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.SetSlot))]
            [HarmonyPostfix]
            public static void LoadOnSlotChange()
            {
                LoadData();
            }

            [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.GetGeneralProgress))]
            [HarmonyPostfix]
            public static void LoadOnStart()
            {
                try
                {
                    LoadData();
                } catch (Exception ex) { Debug.LogError(ex.ToString()); }
            }

            [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))]
            [HarmonyPostfix]
            public static void GiveGuns(GunSetter __instance)
            {
                SlotToList = new List<List<GameObject>>
                {
                    { __instance.GetComponent<GunControl>().slot1 },
                    { __instance.GetComponent<GunControl>().slot2 },
                    { __instance.GetComponent<GunControl>().slot3 },
                    { __instance.GetComponent<GunControl>().slot4 },
                    { __instance.GetComponent<GunControl>().slot5 },
                    { __instance.GetComponent<GunControl>().slot6 }
                };

                foreach (Weapon weapon in WeaponList)
                {
                    bool IsFist = weapon.GetType() == typeof(Fist) || weapon.GetType().IsSubclassOf(typeof(Fist));

                    if (weapon.Enabled() > 0 && !IsFist)
                    {
                        if(GameProgressSaver.CheckGear(weapon.Pref()) == 1)
                        {
                            GameObject created = weapon.Create(__instance.transform);
                            created.SetActive(false);
                            SlotToList[weapon.Slot()].Add(created);
                        } 
                        else
                        {
                            WeaponOwned["weapon." + weapon] = 0;
                        }
                    }
                }

                foreach (List<GameObject> slot in SlotToList)
                {
                    while (slot.Contains(null))
                    {
                        slot.Remove(null);
                    }
                }
            }

            [HarmonyPatch(typeof(FistControl), nameof(FistControl.ResetFists))]
            [HarmonyPostfix]
            public static void GiveFists(FistControl __instance)
            {
                foreach (Weapon weapon in WeaponList)
                {
                    bool IsFist = weapon.GetType() == typeof(Fist) || weapon.GetType().IsSubclassOf(typeof(Fist));

                    if (weapon.Enabled() > 0 && IsFist)
                    {
                        if (GameProgressSaver.CheckGear(weapon.Pref()) == 1)
                        {
                            GameObject created = weapon.Create(__instance.transform);
                            created.SetActive(false);

                            FistControl.Instance.spawnedArms.Add(created);
                            FistControl.Instance.spawnedArmNums.Add(weapon.Slot());
                        }
                        else
                        {
                            WeaponOwned["weapon." + weapon.Pref()] = 0;
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.CheckGear))]
            [HarmonyPrefix]
            public static bool CheckGearForCustoms(ref int __result, string gear)
            {
                foreach (Weapon weapon in WeaponList)
                {
                    if (weapon.Pref() == gear)
                    {
                        __result = WeaponOwned["weapon." + weapon.Pref()];
                        return false;
                    }
                }
                return true;
            }

            [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.AddGear))]
            [HarmonyPrefix]
            public static bool AddGearForCustoms(string gear)
            {
                foreach (Weapon weapon in WeaponList)
                {
                    if (weapon.Pref() == gear)
                    {
                        WeaponOwned["weapon." + weapon.Pref()] = 1;
                        SaveData();
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
