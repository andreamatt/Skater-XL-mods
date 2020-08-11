using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public class CreateAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles() {
		Debug.Log("Building");
		string assetBundleDirectory = "Assets/AssetBundles";
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
		Debug.Log("Done: " + DateTime.Now);
	}

	[MenuItem("Assets/Build AssetBundles to Mod")]
	static void BuildAllAssetBundlesToMod() {
		Debug.Log("Building");
		string assetBundleDirectory = "D:\\Programming\\SXL mods\\Skater XL\\Mods\\XLGraphics";
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
		Debug.Log("Done: " + DateTime.Now);
	}
}
