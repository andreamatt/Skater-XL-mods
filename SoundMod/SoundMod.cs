using SkaterXL.Sound;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace XLSoundMod
{
	public class SoundMod : MonoBehaviour
	{
		private void Start()
		{
			Load();
		}

		private AudioClip GetClip(string path)
		{
			WWW www = new WWW(path);
			while (!www.isDone)
			{
				Thread.Sleep(1);
			}
			return www.GetAudioClip();
		}

		private void Load()
		{
			if (!Directory.Exists(Main.modEntry.Path + "/Sounds"))
			{
				Directory.CreateDirectory(Main.modEntry.Path + "/Sounds");
				Main.Log("Created folder " + Main.modEntry.Path + "/Sounds");
			}

			LoadSounds(ref SoundDatabase.Instance.sounds[0].clips, "bumps*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[1].clips, "ollie_scooped*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[2].clips, "ollie_slow*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[3].clips, "ollie_fast*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[4].clips, "ollie_wood_slow*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[5].clips, "board_land*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[6].clips, "board_wood_land*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[7].clips, "board_impacts*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[8].clips, "board_wood_impacts*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[9].clips, "board_grass_impacts*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[10].clips, "board_tarmac_impacts*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[11].clips, "tutorial_board_impact*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[12].clips, "bearing_sounds*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[13].clips, "shoes_board_back_impacts*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[14].clips, "shoes_impact_ground_sole*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[15].clips, "shoes_impact_ground_upper*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[16].clips, "shoes_movement_short*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[17].clips, "shoes_movement_long*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[18].clips, "shoes_pivot_heavy*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[19].clips, "shoes_pivot_light*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[20].clips, "shoes_push_impact*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[21].clips, "shoes_push_off*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[22].clips, "concrete_grind_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[23].clips, "concrete_grind_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[24].clips, "concrete_grind_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[25].clips, "metal_grind_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[26].clips, "metal_grind_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[27].clips, "metal_grind_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[28].clips, "wood_grind_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[29].clips, "wood_grind_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[30].clips, "wood_grind_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[31].clips, "concrete_powerslide_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[32].clips, "concrete_powerslide_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[33].clips, "concrete_powerslide_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[34].clips, "tarmac_powerslide_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[35].clips, "tarmac_powerslide_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[36].clips, "tarmac_powerslide_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[37].clips, "brick_powerslide_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[38].clips, "brick_powerslide_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[39].clips, "brick_powerslide_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[40].clips, "wood_powerslide_start*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[41].clips, "wood_powerslide_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[42].clips, "wood_powerslide_end*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[43].clips, "movement_foley_jump*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[44].clips, "movement_foley_land*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[45].clips, "grass_roll_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[46].clips, "rolling_sound_slow*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[47].clips, "rolling_sound_fast*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[48].clips, "rolling_brick_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[49].clips, "rolling_tarmac_loop*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[50].clips, "rolling_wood_loop*.wav");

			if (!Directory.Exists(Main.modEntry.Path + "/Sounds/Ragdoll"))
			{
				Directory.CreateDirectory(Main.modEntry.Path + "/Sounds/Ragdoll");
				Main.Log("Created folder " + Main.modEntry.Path + "/Sounds/Ragdoll");
			}

			LoadSounds(ref SoundDatabase.Instance.sounds[51].clips, "Ragdoll/ragdoll_body*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[52].clips, "Ragdoll/ragdoll_legs_drag*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[53].clips, "Ragdoll/ragdoll_legs_hit*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[54].clips, "Ragdoll/ragdoll_metal*.wav");
			LoadSounds(ref SoundDatabase.Instance.sounds[55].clips, "Ragdoll/ragdoll_arms*.wav");

			if (!Directory.Exists(Main.modEntry.Path + "/Sounds/UI"))
			{
				Directory.CreateDirectory(Main.modEntry.Path + "/Sounds/UI");
				Main.Log("Created folder " + Main.modEntry.Path + "/Sounds/UI");
			}

			LoadSounds(ref UISounds.Instance.ui_enter_menu, "UI/ui_enter_menu*.wav");
			LoadSounds(ref UISounds.Instance.ui_select_major, "UI/ui_select_major*.wav");
			LoadSounds(ref UISounds.Instance.ui_select_minor, "UI/ui_select_minor*.wav");
			LoadSounds(ref UISounds.Instance.ui_selection_change, "UI/ui_selection_change*.wav");
			LoadSounds(ref UISounds.Instance.ui_back_exit, "UI/ui_back_exit*.wav");
			LoadSounds(ref UISounds.Instance.MatchedTrickSounds, "UI/matched_trick_sound*.wav");
			LoadSounds(ref UISounds.Instance.FailedTrickSounds, "UI/failed_trick_sound*.wav");

			UISounds.Instance.uiSource.mute = false;
			UISounds.Instance.uiSource.volume = 1;
		}

		private void LoadSound(ref AudioClip audioClip, string name)
		{
			string path = Main.modEntry.Path + "/Sounds/" + name;
			if (File.Exists(path))
			{
				AudioClip clip = this.GetClip(path);
				clip.name = name;
				audioClip = clip;
				return;
			}
			Main.Log("No " + name + " found");
		}

		private void LoadSounds(ref AudioClip[] audioClips, string pattern)
		{
			string[] array = (from s in Directory.GetFiles(Main.modEntry.Path + "/Sounds/", pattern)
							  orderby s
							  select s).ToArray<string>();
			List<AudioClip> list = new List<AudioClip>();
			foreach (string text in array)
			{
				AudioClip clip = this.GetClip(text);
				clip.name = text;
				list.Add(clip);
			}
			if (list.Count > 0)
			{
				if (Regex.Match(pattern, "rolling_(brick|wood)_loop.*").Success && list.Count == 1)
				{
					list.Add(list[0]);
				}
				audioClips = list.ToArray();
				return;
			}
			Main.Log("No " + pattern + " found");
		}
	}
}