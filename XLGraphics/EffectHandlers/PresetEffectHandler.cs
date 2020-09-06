using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLGraphics.Presets;
using XLGraphicsUI.Elements;

namespace XLGraphics.EffectHandlers
{
	abstract public class PresetEffectHandler : EffectHandler
	{
		abstract public void OnChangeSelectedPreset(Preset preset);
	}
}
