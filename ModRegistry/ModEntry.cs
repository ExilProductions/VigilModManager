using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VMM.ModRegistry.Settings;

namespace VMM.ModRegistry
{
    public class ModEntry
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public ModSettings Settings { get; set; }

        internal Assembly assembly { get; set; }
    }
}
