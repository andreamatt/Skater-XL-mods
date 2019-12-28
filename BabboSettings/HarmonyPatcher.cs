using BabboSettings.Patches;
using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BabboSettings
{
    static class HarmonyPatcher
    {
        [Flags]
        enum PatchType
        {
            None = 0,
            Prefix = 1,
            Postfix = 2,
            Transpiler = 4
        }   // for multiple ones use bitwise OR => Prefix | Postfix

        static private Dictionary<Type, PatchType> AlwaysPatch = new Dictionary<Type, PatchType>() {
            { typeof(PlayerController_IsSwitch_Patch), PatchType.Postfix },
            { typeof(PlayerController_IsAnimSwitch_Patch), PatchType.Postfix },
            { typeof(Respawn_DoRespawn_Patch), PatchType.Prefix },
            { typeof(Respawn_SetSpawnPos_Patch), PatchType.Prefix }
        };
        static private Dictionary<Type, PatchType> BetaPatch = new Dictionary<Type, PatchType>() {
            { typeof(Respawn_SetSpawnPos_Patch_Vector_Quaternion), PatchType.Prefix },
            { typeof(Respawn_EndRespawning_Patch), PatchType.Postfix }
        };
        static private Dictionary<Type, PatchType> NonBetaPatch = new Dictionary<Type, PatchType>() {
        };

        static private List<Tuple<MethodBase, HarmonyMethod, HarmonyMethod, HarmonyMethod>> ToPatch;

        static private void Initialize() {
            Logger.Log("Game version: " + Application.version); //0.1.0.0b9 = beta

            // sum patches based on game version
            var patches = new Dictionary<Type, PatchType>();
            foreach (var p in AlwaysPatch) {
                patches.Add(p.Key, p.Value);
            }
            if (Application.version == Main.BetaVersion) {
                foreach (var p in BetaPatch) {
                    patches.Add(p.Key, p.Value);
                }
            }
            else if (Application.version == Main.NonBetaVersion) {
                foreach (var p in NonBetaPatch) {
                    patches.Add(p.Key, p.Value);
                }
            }
            else {
                Logger.Log("ERROR!! UNKOWN GAME VERSION: " + Application.version);
            }

            // collect all the patches
            ToPatch = new List<Tuple<MethodBase, HarmonyMethod, HarmonyMethod, HarmonyMethod>>();
            foreach (var item in patches) {
                var attributes = item.Key.GetCustomAttribute<HarmonyPatch>();
                var originalMethodName = attributes.info.methodName;
                var originalMethodTypes = attributes.info.argumentTypes;
                MethodBase original;
                if (originalMethodTypes == null) {
                    original = attributes.info.declaringType.GetMethod(originalMethodName, originalMethodTypes);
                }
                else {
                    original = attributes.info.declaringType.GetMethod(originalMethodName);
                }

                var patchType = item.Value;
                HarmonyMethod prefix = null, postfix = null, transpiler = null;
                if (patchType.HasFlag(PatchType.Prefix)) {
                    prefix = new HarmonyMethod(item.Key.GetMethod("Prefix"));
                }
                if (patchType.HasFlag(PatchType.Postfix)) {
                    postfix = new HarmonyMethod(item.Key.GetMethod("Postfix"));
                }
                if (patchType.HasFlag(PatchType.Transpiler)) {
                    transpiler = new HarmonyMethod(item.Key.GetMethod("Transpiler"));
                }

                ToPatch.Add(new Tuple<MethodBase, HarmonyMethod, HarmonyMethod, HarmonyMethod>(original, prefix, postfix, transpiler));
            }
        }

        static public void Patch(HarmonyInstance harmonyInstance) {
            if (ToPatch == null) {
                Initialize();
            }

            foreach (var item in ToPatch) {
                harmonyInstance.Patch(item.Item1, item.Item2, item.Item3, item.Item4);
            }
        }

        static public void UnPatch(HarmonyInstance harmonyInstance) {
            foreach (var item in ToPatch) {
                var prefix = item.Item2;
                var postfix = item.Item3;
                var transpiler = item.Item4;
                if (prefix != null) {
                    harmonyInstance.Unpatch(item.Item1, prefix.method);
                }
                if (postfix != null) {
                    harmonyInstance.Unpatch(item.Item1, postfix.method);
                }
                if (transpiler != null) {
                    harmonyInstance.Unpatch(item.Item1, transpiler.method);
                }
            }
        }
    }
}
