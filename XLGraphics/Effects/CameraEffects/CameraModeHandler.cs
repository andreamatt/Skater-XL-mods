using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Effects.CameraEffects
{
	public class CameraModeHandler : EffectHandler
	{
		public override void ConnectUI() {
			throw new NotImplementedException();
		}
	}

	public enum CameraMode
	{
		Normal,
		Low,
		Follow,
		POV,
		Skate
	}
}
