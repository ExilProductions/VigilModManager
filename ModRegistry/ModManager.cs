using MelonLoader;
using System.Reflection;
using VMM.ModRegistry.Settings;
using VMM.UI;

namespace VMM.ModRegistry
{
    public class ModManager
    {
        public static ModManager Instance { get; internal set; }

        public Action onFinishedLoading;

        internal List<ModEntry> LoadedMods = new List<ModEntry>();

        public ModManager()
        {
            if (Instance == null)
                Instance = this;
            else
                Core.Logger.Warning("Duplicate ModManager Detected");
        }

        internal void LoadMods()
        {
            Core.Logger.Msg("=== Installed Melon Mods ===");
            foreach (var mod in MelonMod.RegisteredMelons.OrderBy(m => m.Info.Name))
            {
                Core.Logger.Msg($"Name: {mod.Info.Name} | Version: {mod.Info.Version} | Author: {mod.Info.Author}");

                var modEntry = new ModEntry
                {
                    Name = mod.Info.Name,
                    Version = mod.Info.Version,
                    Author = mod.Info.Author,
                    assembly = mod.GetType().Assembly,
                    Settings = null
                };

                LoadedMods.Add(modEntry);
                UIManager.Instance.AddModButton(modEntry);
            }
            Core.Logger.Msg("============================");
            onFinishedLoading?.Invoke();
        }

        public ModEntry GetModEntryByName(string modName)
        {
            return LoadedMods.FirstOrDefault(m =>
                m.Name.Equals(modName, StringComparison.OrdinalIgnoreCase));
        }

        public List<ModEntry> GetLoadedMods() => LoadedMods;

        public void RegisterSettings(Assembly modAssembly, ModSettings settings)
        {
            var mod = LoadedMods.Find(m => m.assembly == modAssembly);
            if (mod != null)
            {
                mod.Settings = new ModSettings();
                mod.Settings = settings;
                Core.Logger.Msg($"Registered settings for mod: {mod.Name}");
            }
            else
            {
                Core.Logger.Error("Mod not found for the provided assembly.");
            }
        }
    }
}
