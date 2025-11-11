namespace VMM.ModRegistry.Settings
{
    public class ModSettings
    {
        private readonly List<ISettingsElement> settings = new();

        public void AddSetting(ISettingsElement newSetting)
        {
            settings.Add(newSetting);
        }

        public T GetSetting<T>(string name) where T : class, ISettingsElement
        {
            foreach (var setting in settings)
            {
                if(setting.Name == name && setting is T typed)
                    return typed;
            }
            return null;
        }

        public IEnumerable<ISettingsElement> GetAllSettings() => settings;
    }
}
