using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLGraphics.EffectHandlers.CameraEffects;

namespace XLGraphics.SerializationData
{
	public class CameraData
	{
		// Camera
		public XLCameraMode CAMERA_MODE = XLCameraMode.Normal;

		public float REPLAY_FOV = 60;

		public float NORMAL_FOV = 60;
		public float NORMAL_REACT = 0.90f;
		public float NORMAL_REACT_ROT = 0.90f;
		public float NORMAL_CLIP = 0.01f;

		public float FOLLOW_FOV = 60;
		public float FOLLOW_REACT = 0.70f;
		public float FOLLOW_REACT_ROT = 0.70f;
		public float FOLLOW_CLIP = 0.01f;
		public SerializableVector3 FOLLOW_SHIFT = Vector3.zero;
		public bool FOLLOW_AUTO_SWITCH = false;

		public float POV_FOV = 80;
		public float POV_REACT = 1;
		public float POV_REACT_ROT = 0.07f;
		public float POV_CLIP = 0.01f;
		public bool HIDE_HEAD = true;
		public SerializableVector3 POV_SHIFT = new Vector3(0, 0, 0.13f);

		public float SKATE_FOV = 60;
		public float SKATE_REACT = 0.90f;
		public float SKATE_REACT_ROT = 0.90f;
		public float SKATE_CLIP = 0.01f;
		public SerializableVector3 SKATE_SHIFT = Vector3.zero;
	}
}
