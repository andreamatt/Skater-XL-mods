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
	
	[HarmonyPatch(typeof(EventManager), "ExitGrind")]
    static class EventManager_ExitGrind_Patch
    {
		static void Postfix()
		{
	    		PlayerController.Instance.boardController.isSliding = false; // Force it for sliding out without popping.
		}
    }

	[HarmonyPatch(typeof(DeckSounds), "SetRollingVolFromRPS")]
	static class DeckSounds_SetRollingVolFromRPS_Patch
	{
		static bool Prefix(ref PlayerController.SurfaceTags _surfaceTag, ref float rps)
		{
			LevelSoundManager lvlManager = GameObject.FindObjectOfType<LevelSoundManager>();  // Always grab the first component
			if (!lvlManager) return true;  // Continue to original function, collection does not exist.

			PlayerController player = PlayerController.Instance;
			RaycastHit hit;
			Vector3 direction = -player.skaterController.skaterTransform.up;
			Ray ray = new Ray(player.boardController.boardTransform.position, direction);

			if (!Physics.Raycast(ray, out hit)) return true; // Continue to original function, no ray hit no object below us.

			MeshCollider meshCollider = hit.collider as MeshCollider;
			MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();

			if (!meshCollider || !meshCollider.sharedMesh) return true;  // Continue to original function if we don't have a mesh collider we can't continue.

			Mesh mesh = meshCollider.sharedMesh;
			if (mesh.subMeshCount <= 1) return true;  // Continue to original function. Single submesh means there's no alternative material.

			int triangleIdx = hit.triangleIndex;
			int[] lookupIdx = new int[3] { mesh.triangles[triangleIdx * 3], mesh.triangles[triangleIdx * 3 + 1], mesh.triangles[triangleIdx * 3 + 2] };

			int submeshCount = mesh.subMeshCount;

			for (int i = 0; i < submeshCount; ++i)
			{
				int[] tris = mesh.GetTriangles(i);
				for (int j = 0; j < tris.Length; j += 3)
				{
					if (tris[j] == lookupIdx[0] &&
						tris[j + 1] == lookupIdx[1] &&
						tris[j + 2] == lookupIdx[2])
					{
						// Get the surface tag from the scriptable object by material
						int tag = lvlManager.tagCollection.GetSurfaceTagByMaterial(meshRenderer.sharedMaterials[i]);
						_surfaceTag = tag == 0 ? _surfaceTag : (PlayerController.SurfaceTags)tag;
						return true;
					}
				}
			}

			return true;
		}
	}
}
