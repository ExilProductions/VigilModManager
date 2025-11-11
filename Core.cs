using MelonLoader;
using System.Collections;
using System.Reflection;
using UnityEngine;
using VMM;
using VMM.ModRegistry;
using VMM.ModRegistry.Settings;
using VMM.ModRegistry.Settings.Types;
using VMM.UI;

[assembly: MelonInfo(typeof(VMM.Core), ModInfo.Name, ModInfo.Version, ModInfo.Author, ModInfo.DownloadLink)]
[assembly: MelonGame("Singularity Studios", "Vigil")]

namespace VMM
{
    internal class Core : MelonMod
    {
        public static MelonLogger.Instance Logger
        {
            get
            {
                if (_logger != null)
                {
                    return _logger;
                }
                else
                {
                    throw new System.Exception("Logger instance is not initialized yet.");
                }
            }
        }

        private static MelonLogger.Instance _logger;

        public override void OnInitializeMelon()
        {
            _logger = LoggerInstance;
            LoggerInstance.Msg("Initialized.");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != 0) return; // Main Menu only

            InitializeManagers();
            SetupModLoading();
        }

        public override void OnApplicationQuit()
        {
            Cleanup();
        }

        private void InitializeManagers()
        {
            UIManager.Instance = new UIManager();
            UIManager.Instance.Initialize();

            ModManager.Instance = new ModManager();
        }

        private void SetupModLoading()
        {
            //ModManager.Instance.onFinishedLoading += OnLoadedMods;
            ModManager.Instance.LoadMods();
        }

        private void Cleanup()
        {
            if (ModManager.Instance != null)
            {
                //ModManager.Instance.onFinishedLoading -= OnLoadedMods;
            }
        }

        //test setting for testing
        //private void OnLoadedMods()
        //{
        //    var settings = new ModSettings();
        //    var testToggle = new ToggleSetting
        //    {
        //        Name = "Test Toggle",
        //        Value = true,
        //        OnChanged = value => LoggerInstance.Msg($"Test Toggle pressed! State {value}")
        //    };
        //    settings.AddSetting(testToggle);
        //
        //    ModManager.Instance.RegisterSettings(Assembly.GetExecutingAssembly(), settings);
        //}
    }
}