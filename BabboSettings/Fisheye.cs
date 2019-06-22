
using System;
using UnityEngine;

namespace BabboSettings {
	public class Fisheye : MonoBehaviour{
		private Shader shader;
		private Material material;

		public Fisheye() {
			Main.log("Initiating fisheye");
			try {
				// load shader
				var bundle = AssetBundle.LoadFromFile("D:/Steam/steamapps/common/Skater XL/SkaterXL_Data/Resources/fisheye_asset");
				Main.log("Bundle: " + bundle.name);
				shader = bundle.LoadAsset<Shader>("Fisheye");
				Main.log("Shader: " + shader.name);

				Main.log("Supported: " + shader.isSupported);

				// material
				material = new Material(shader);
			}
			catch (Exception e) {
				Main.log("Failed loading shader, e: " + e);
			}
		}

		public void OnRenderImage(RenderTexture src, RenderTexture dest) {
			Main.log("rendering");
			if (material != null) Graphics.Blit(src, dest, material);
			else Main.log("Material is null");
		}
	}
}
