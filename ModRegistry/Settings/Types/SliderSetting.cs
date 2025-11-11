using System;
using System.Collections.Generic;
using System.Text;

namespace VMM.ModRegistry.Settings.Types
{
    public class SliderSetting : SettingsElement<float>
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }
}
