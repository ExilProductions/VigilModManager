using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VMM.ModRegistry;
using VMM.ModRegistry.Settings.Types;
using VMM.UI.Helpers;

namespace VMM.UI.Components
{
    internal class ModSettingsManager : MonoBehaviour
    {
        public static ModSettingsManager Instance;

        public TextMeshProUGUI titleText;

        RectTransform settingsContainer;

        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                settingsContainer = transform.GetChild(0).GetComponent<RectTransform>();
                settingsContainer.gameObject.SetActive(false);
            }
            else
            {
                Core.Logger.Warning("Form some reason there's a second instance for the ModSettingsManager");
                Destroy(this);
            }
        }

        void OnDisable()
        {
            ClearSettings();
        }

        void ClearSettings()
        {
            for(int i = 0; i < settingsContainer.childCount; i++)
            {
                Destroy(settingsContainer.GetChild(i).gameObject);
            }
        }

        public void OpenSettings(ModEntry mod)
        {
            if(mod.Settings == null)
            {
                Core.Logger.Warning($"Mod '{mod.Name}' Does not Contain Any Settings");
                return;
            }
            ClearSettings();
            var settings = mod.Settings.GetAllSettings();
            if (settings != null && settings.Count() <= 0)
            {
                Core.Logger.Warning($"Settings Reference of '{mod.Name}' were found but no Settings were found inside List");
                return;
            }
            titleText.text = $"{mod.Name} Settings";
            foreach(var setting in settings)
            {
                if(setting is SliderSetting slider)
                    UIBuilder.CreateSlider("Slider Setting", settingsContainer, slider.Name, slider.Max, slider.Min, slider.Value, (float value) => slider.OnChanged.Invoke(value));
                if (setting is ToggleSetting toggle)
                    UIBuilder.CreateToggle("Toggle Setting", settingsContainer, toggle.Name, toggle.Value, (bool value) => toggle.OnChanged.Invoke(value));
            }
            settingsContainer.gameObject.SetActive(true);
            UIManager.Instance.OpenSettings();
        }

    }
}
