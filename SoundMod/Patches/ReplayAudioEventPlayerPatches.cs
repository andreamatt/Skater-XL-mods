using Harmony12;
using ReplayEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SoundMod.Patches
{
    [HarmonyPatch(typeof(ReplayAudioEventPlayer), "SetPlaybackTime")]
    static class SetPlaybackTime_Patch
    {
        static bool slow_done = false;
        static bool fast_done = false;

        static bool Prefix(ReplayAudioEventPlayer __instance, float time, float timeScale) {
            //Logger.Log("Started prefix replayaudioeventplayer");
            // only if they are still missing
            if (!slow_done || !fast_done) {
                var audioSource = Traverse.Create(__instance).Field("m_AudioSource").GetValue<AudioSource>();
                if (audioSource.name == "Wheel_Rolling_Loop_Low_Source" && !slow_done) {
                    Logger.Log("Replacing low");
                    slow_done = true;
                    audioSource.Stop();
                    audioSource.clip = DeckSounds.Instance.rollingSoundSlow;
                    audioSource.Play();
                    Logger.Log("Replaced low");
                }
                if (audioSource.name == "Wheel_Rolling_Loops_High_Source" && !fast_done) {
                    Logger.Log("Replacing high");
                    fast_done = true;
                    audioSource.Stop();
                    audioSource.clip = DeckSounds.Instance.rollingSoundFast;
                    audioSource.Play();
                    Logger.Log("Replaced high");
                }
            }
            return true;
        }
    }
}
