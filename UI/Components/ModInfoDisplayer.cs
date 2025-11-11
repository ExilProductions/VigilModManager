using TMPro;
using UnityEngine;
using VMM.ModRegistry;

namespace VMM.UI.Components
{
    internal class ModInfoDisplayer : MonoBehaviour
    {
        internal static ModInfoDisplayer Instance;

        internal TextMeshProUGUI modName;
        internal TextMeshProUGUI modAuthor;
        internal TextMeshProUGUI modVersion;
        internal UnityEngine.UI.Button settingsButton;
        internal ModEntry currentMod;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                settingsButton.onClick.AddListener(OnSettingsButtonPressed);
                transform.gameObject.SetActive(false);
            }
            else
            {
                Destroy(this);
                Core.Logger.Warning("For some reason there were two Intances of ModInfoDisplayer???");
            }
        }

        public void UpdateModInfo(ModEntry mod)
        {
            if(mod.Settings == null)
            {
                settingsButton.gameObject.SetActive(false);
            }
            else if(mod.Settings != null && !settingsButton.gameObject.activeSelf)
            {
                settingsButton.gameObject.SetActive(true);
            }

            transform.gameObject.SetActive(true);
            modName.text = $"Name: {mod.Name}";
            modAuthor.text = $"Author: {mod.Author}";
            modVersion.text = $"Version: {mod.Version}";
            currentMod = mod;
        }

        public void HandleClose()
        {
            if(gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        void OnSettingsButtonPressed()
        {
            if(currentMod == null)
                return;

            ModSettingsManager.Instance.OpenSettings(currentMod);
        }
    }
}
