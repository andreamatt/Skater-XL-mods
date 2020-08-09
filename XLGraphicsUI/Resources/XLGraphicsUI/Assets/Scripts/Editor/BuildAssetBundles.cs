using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public class CreateAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles() {
		string assetBundleDirectory = "Assets/AssetBundles";
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
		Debug.Log("Done: " + DateTime.Now);
	}

	[MenuItem("Assets/Build AssetBundles to Mod")]
	static void BuildAllAssetBundlesToMod() {
		string assetBundleDirectory = "D:\\Steam\\steamapps\\common\\Skater XL\\Mods\\XLGraphics";
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
		Debug.Log("Done: " + DateTime.Now);
	}
}
