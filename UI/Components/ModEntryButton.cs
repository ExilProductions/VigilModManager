using FMODUnity;
using UnityEngine;
using VMM.ModRegistry;

namespace VMM.UI.Components
{
    internal class ModEntryButton : MonoBehaviour
    {
        public ModEntry mod;

        UnityEngine.UI.Button button;
        EventReference buttonPress;

        void Awake()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(ShowModInfo);
            buttonPress = VMM.Patches.MainMenuManager.GetButtonPress(global::MainMenuManager.instance);
        }

        void ShowModInfo()
        {
            ModInfoDisplayer.Instance.UpdateModInfo(mod);
            AudioManager.instance.PlayOneShot(buttonPress, transform.position);
        }
    }
}
