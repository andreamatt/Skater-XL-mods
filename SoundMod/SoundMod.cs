using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace SoundMod
{
	public class SoundMod : MonoBehaviour
	{
		Dictionary<string, AudioClip> clipForName;

		public void Start() {
			Logger.Log("Changed decksounds instance");
			var sound = SoundManager.Instance;
			var soundTraverse = Traverse.Create(sound);

			clipForName = soundTraverse.Field("clipForName").GetValue<Dictionary<string, AudioClip>>();
			if (clipForName != null) Load();
			else Logger.Log("clipForName instance is null");
		}

		int loaded_sounds = 0;
		private void Load() {
			if (clipForName == null) {
				Logger.Log("clipForName instance is null");
				return;
			}

			loaded_sounds = 0;
			Logger.Log("Loading sounds...");

			var deckSounds = SoundManager.Instance.deckSounds;			

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
			LoadSounds(ref deckSounds.ollieWoodSlow, "ollie_wood_slow*.wav");
			LoadSounds(ref deckSounds.ollieFast, "ollie_fast*.wav");
			LoadSounds(ref deckSounds.boardLand, "board_land*.wav");
			LoadSounds(ref deckSounds.boardWoodLand, "board_wood_land*.wav");
			LoadSounds(ref deckSounds.boardImpacts, "board_impacts*.wav");
			LoadSounds(ref deckSounds.boardWoodImpacts, "board_wood_impacts*.wav");
			LoadSounds(ref deckSounds.boardTarmacImpacts, "board_tarmac_impacts*.wav");
			LoadSounds(ref deckSounds.boardGrassImpacts, "board_grass_impacts*.wav");
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
			LoadSounds(ref deckSounds.concretePowerslideStart, "concrete_powerslide_start*.wav");
			LoadSounds(ref deckSounds.concretePowerslideLoop, "concrete_powerslide_loop*.wav");
			LoadSounds(ref deckSounds.concretePowerslideEnd, "concrete_powerslide_end*.wav");
			LoadSounds(ref deckSounds.tarmacPowerslideStart, "tarmac_powerslide_start*.wav");
			LoadSounds(ref deckSounds.tarmacPowerslideLoop, "tarmac_powerslide_loop*.wav");
			LoadSounds(ref deckSounds.tarmacPowerslideEnd, "tarmac_powerslide_end*.wav");
			LoadSounds(ref deckSounds.brickPowerslideStart, "brick_powerslide_start*.wav");
			LoadSounds(ref deckSounds.brickPowerslideLoop, "brick_powerslide_loop*.wav");
			LoadSounds(ref deckSounds.brickPowerslideEnd, "brick_powerslide_end*.wav");
			LoadSounds(ref deckSounds.woodPowerslideStart, "wood_powerslide_start*.wav");
			LoadSounds(ref deckSounds.woodPowerslideLoop, "wood_powerslide_loop*.wav");
			LoadSounds(ref deckSounds.woodPowerslideEnd, "wood_powerslide_end*.wav");
			LoadSounds(ref deckSounds.movement_foley_jump, "movement_foley_jump*.wav");
			LoadSounds(ref deckSounds.movement_foley_land, "movement_foley_land*.wav");
			LoadSounds(ref deckSounds.rollingBrickLoop, "rolling_brick_loop*.wav");
			LoadSounds(ref deckSounds.rollingWoodLoop, "rolling_wood_loop*.wav");
			LoadSounds(ref deckSounds.rollingTarmacLoop, "rolling_tarmac_loop*.wav");
			deckSounds.GenerateInitialEvents();

			Logger.Log($"{loaded_sounds} sounds loaded");

			Traverse.Create(SoundManager.Instance).Method("UpdateAudioClipDictionaries").GetValue();

			Logger.Log($"{loaded_sounds} sounds applied");
		}

		private void LoadSounds(ref AudioClip[] audioClips, string pattern) {
			//Logger.Log("Getting files for " + pattern);
			string[] filePaths = Directory.GetFiles($"{Main.modEntry.Path}/Sounds/", pattern).OrderBy(s => s).ToArray();

			List<AudioClip> audioClipList = new List<AudioClip>();
			foreach (var filePath in filePaths) {
				var clip = GetClip(filePath);
				clip.name = filePath;

				clipForName[clip.name] = clip;

				audioClipList.Add(clip);
				loaded_sounds++;
			}

			if (audioClipList.Count > 0) {
				// if it's one of the special rolling loops and there is only 1 value, duplicate it
				if (Regex.Match(pattern, @"rolling_(brick|wood)_loop.*").Success && audioClipList.Count == 1) {
					audioClipList.Add(audioClipList[0]);
					Logger.Log("Duplicating " + pattern + " sound");
				}
				audioClips = audioClipList.ToArray();
			}
			else {
				Logger.Log("No " + pattern + " found");
			}
		}

		private void LoadSound(ref AudioClip audioClip, string name) {
			var path = $"{Main.modEntry.Path}/Sounds/{name}";
			if (File.Exists(path)) {
				var clip = GetClip(path);
				clip.name = name;
				clipForName[clip.name] = clip;

				audioClip = clip;
				loaded_sounds++;
			}
			else {
				Logger.Log("No " + name + " found");
			}
		}

		private AudioClip GetClip(string path) {
			WWW audioLoader = new WWW(path);
			while (!audioLoader.isDone) System.Threading.Thread.Sleep(1);
			return audioLoader.GetAudioClip();
		}
	}
}
