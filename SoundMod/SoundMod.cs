using Harmony12;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XLShredLib;
using XLShredLib.UI;
using System.Linq;
using ReplayEditor;

namespace SoundMod
{
    public static class SoundMod
    {
        static Dictionary<string, AudioClip> clipForName;
        static DeckSounds deckSounds;

        static public void Start() {
            deckSounds = DeckSounds.Instance;

            var deckSounds_traverse = Traverse.Create(deckSounds);
            clipForName = deckSounds_traverse.Field("clipForName").GetValue<Dictionary<string, AudioClip>>();

            // get new rolling sounds
            LoadSound(ref deckSounds.rollingSoundSlow, "rolling_sound_slow.wav");
            LoadSound(ref deckSounds.rollingSoundFast, "rolling_sound_fast.wav");

            // replace source clip
            deckSounds.wheelRollingLoopLowSource.clip = deckSounds.rollingSoundSlow;
            deckSounds.wheelRollingLoopHighSource.clip = deckSounds.rollingSoundFast;

            // start playing
            deckSounds.wheelRollingLoopLowSource.Play();
            deckSounds.wheelRollingLoopHighSource.Play();


            // load single sounds
            LoadSound(ref deckSounds.tutorialBoardImpact, "tutorial_board_impact.wav");
            LoadSound(ref deckSounds.grassRollLoop, "grass_roll_loop.wav");

            // load arrays
            LoadSounds(ref deckSounds.bumps, "bumps*.wav");
            LoadSounds(ref deckSounds.ollieScooped, "ollie_scooped*.wav");
            LoadSounds(ref deckSounds.ollieSlow, "ollie_slow*.wav");
            LoadSounds(ref deckSounds.ollieFast, "ollie_fast*.wav");
            LoadSounds(ref deckSounds.boardLand, "board_land*.wav");
            LoadSounds(ref deckSounds.boardImpacts, "board_impacts*.wav");
            LoadSounds(ref deckSounds.bearingSounds, "bearing_sounds*.wav");
            LoadSounds(ref deckSounds.shoesBoardBackImpacts, "shoes_board_back_impacts*.wav");
            LoadSounds(ref deckSounds.shoesImpactGroundSole, "shoes_impact_ground_sole*.wav");
            LoadSounds(ref deckSounds.shoesImpactGroundUpper, "shoes_impact_ground_upper*.wav");
            LoadSounds(ref deckSounds.shoesMovementShort, "shoes_movement_short*.wav");
            LoadSounds(ref deckSounds.shoesMovementLong, "shoes_movement_long*.wav");
            LoadSounds(ref deckSounds.shoesPivotHeavy, "shoes_pivot_heavy*.wav");
            LoadSounds(ref deckSounds.shoesPivotLight, "shoes_pivot_light*.wav");
            LoadSounds(ref deckSounds.shoesPushImpact, "shoes_push_impact*.wav");
            LoadSounds(ref deckSounds.shoesPushOff, "shoes_push_off*.wav");
            LoadSounds(ref deckSounds.concreteGrindGeneralStart, "concrete_grind_start*.wav");
            LoadSounds(ref deckSounds.concreteGrindGeneralLoop, "concrete_grind_loop*.wav");
            LoadSounds(ref deckSounds.concreteGrindGeneralEnd, "concrete_grind_end*.wav");
            LoadSounds(ref deckSounds.metalGrindGeneralStart, "metal_grind_start*.wav");
            LoadSounds(ref deckSounds.metalGrindGeneralLoop, "metal_grind_loop*.wav");
            LoadSounds(ref deckSounds.metalGrindGeneralEnd, "metal_grind_end*.wav");
            LoadSounds(ref deckSounds.woodGrindGeneralStart, "wood_grind_start*.wav");
            LoadSounds(ref deckSounds.woodGrindGeneralLoop, "wood_grind_loop*.wav");
            LoadSounds(ref deckSounds.woodGrindGeneralEnd, "wood_grind_end*.wav");

            Logger.Log("Sounds loaded");
        }

        static private void LoadSounds(ref AudioClip[] audioClips, string pattern) {
            //Logger.Log("Getting files for " + pattern);
            string[] filePaths = Directory.GetFiles($"{Main.modEntry.Path}/Sounds/", pattern);

            List<AudioClip> audioClipList = new List<AudioClip>();
            foreach (var filePath in filePaths) {
                //Logger.Log("Getting " + filePath);
                var clip = GetClip(filePath);
                clip.name = filePath;
                //Logger.Log("got " + filePath);

                clipForName[clip.name] = clip;
                //clipForName.Add(clip.name, clip);
                //Logger.Log("added " + filePath);

                audioClipList.Add(clip);
                //Logger.Log("added " + filePath);
            }

            if (audioClipList.Count > 0) {
                audioClips = audioClipList.ToArray();
            }
            else {
                Logger.Log("No " + pattern + " found");
            }
        }

        static private void LoadSound(ref AudioClip audioClip, string name) {
            var path = $"{Main.modEntry.Path}/Sounds/{name}";
            if (File.Exists(path)) {
                var clip = GetClip(path);
                //Logger.Log("got " + name + "clipname:" + clip.name);
                clip.name = name;
                clipForName[clip.name] = clip;
                //clipForName.Add(clip.name, clip);
                //Logger.Log("added " + name);

                audioClip = clip;
            }
            else {
                Logger.Log("No " + name + " found");
            }
        }

        static private AudioClip GetClip(string path) {
            WWW audioLoader = new WWW(path);
            while (!audioLoader.isDone) System.Threading.Thread.Sleep(10);
            return audioLoader.GetAudioClip();
        }
    }
}

