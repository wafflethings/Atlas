using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AtlasLib.Utils
{
    public class PatchThis : Attribute
    {
        public static Dictionary<Type, PatchThis> AllPatches = new();
        public static bool HasntPatched = true;

        public static void AddPatches()
        {
            foreach (Type t in Assembly.GetCallingAssembly().GetTypes().Where(t => t.GetCustomAttribute<PatchThis>() != null))
            {
                Debug.Log($"Adding patch {t.Name}...");
                AllPatches.Add(t, t.GetCustomAttribute<PatchThis>());
            }
        }

        public static void PatchAll()
        {
            foreach (KeyValuePair<Type, PatchThis> kvp in AllPatches)
            {
                PatchThis pt = kvp.Value;
                Debug.Log($"Patching {kvp.Key.Name}...");
                pt._harmony.PatchAll(kvp.Key);
            }

            HasntPatched = false;
        }

        private Harmony _harmony;
        public readonly string Name;

        public PatchThis(string harmonyName)
        {
            _harmony = new(harmonyName);
            Name = harmonyName;
        }
    }
}