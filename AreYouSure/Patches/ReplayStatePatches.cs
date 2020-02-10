using GameManagement;
using Harmony12;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AreYouSure.Patches
{

    [HarmonyPatch(typeof(ReplayState), "OnUpdate")]
    static class ReplayState_OnUpdate_Patch
    {
        public static Traverse replayStateTraverse;
        public static bool checkingIfSure;
        static bool Prefix(ReplayState __instance) {
            if (replayStateTraverse == null) {
                replayStateTraverse = Traverse.Create(__instance);
            }

            if (PlayerController.Instance.inputController.player.GetButtonDown("B") && !ReplayEditorController.Instance.Menu.isActiveAndEnabled) {
                if (ReplayEditorController.Instance.ClipEditMode) {
                    //nothing
                }
                else {
                    Logger.Log("Are you sure???");
                    checkingIfSure = true;
                    //__instance.RequestTransitionTo(typeof(PauseState));
                }
            }

            return false;
        }
    }
}
