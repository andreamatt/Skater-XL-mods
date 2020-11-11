using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SoundMod.Patches
{
	[HarmonyPatch(typeof(DeckSounds), "DoBoardImpactGround")]
	static class DeckSounds_DoBoardImpactGround_Patch
	{
		static public Traverse deckSoundsTraverse;
		static bool Prefix(DeckSounds __instance, float vol) {
			if (deckSoundsTraverse == null) {
				deckSoundsTraverse = Traverse.Create(DeckSounds.Instance);
			}

			vol *= 0.032f;

			var boardImpact = deckSoundsTraverse.Field<bool>("_boardImpactAllowed");
			if (boardImpact.Value && vol > 0.01f) {
				// change volume
				vol = Mathf.Clamp(vol, 0.6f, 1.2f);

				var PlayRandomMethod = deckSoundsTraverse.Method("PlayRandomOneShotFromArray", new Type[] { typeof(AudioClip[]), typeof(AudioSource), typeof(float) });
				switch (PlayerController.Instance.GetSurfaceTag(PlayerController.Instance.boardController.GetSurfaceTagString())) {
					case PlayerController.SurfaceTags.None:
					case PlayerController.SurfaceTags.Concrete:
					case PlayerController.SurfaceTags.Brick:
						PlayRandomMethod.GetValue(__instance.boardImpacts, __instance.deckSource, vol);
						break;
					case PlayerController.SurfaceTags.Tarmac:
						PlayRandomMethod.GetValue(__instance.boardTarmacImpacts, __instance.deckSource, vol);
						break;
					case PlayerController.SurfaceTags.Wood:
						PlayRandomMethod.GetValue(__instance.boardWoodImpacts, __instance.deckSource, vol);
						break;
					case PlayerController.SurfaceTags.Grass:
						PlayRandomMethod.GetValue(__instance.boardGrassImpacts, __instance.deckSource, vol);
						break;
				}

				boardImpact.Value = false;

				// change reset time
				__instance.Invoke("ResetBoardImpactAllowed", 0.1f);
			}

			return false;
		}
	}
	
	[HarmonyPatch(typeof(DeckSounds), "DoOllie")]
	static class DeckSounds_DoOllie_Patch
	{
		static bool Prefix(DeckSounds __instance, float scoop) {	
			if (PlayerController.Instance.boardController.isSliding) {
				PlayerController.Instance.boardController.isSliding = false;
				return false;
			}
			return true;
		}
	}
}
