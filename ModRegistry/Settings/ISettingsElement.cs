using System;
using System.Collections.Generic;
using System.Text;

namespace VMM.ModRegistry.Settings
{
    public interface ISettingsElement
    {
        string Name { get; set; }
        object GetValue();
        void SetValue(object value);
    }
}
