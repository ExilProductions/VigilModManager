using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VMM.ModRegistry;
using VMM.UI.Components;
using VMM.UI.Helpers;

namespace VMM.UI
{
    internal class UIManager
    {
        public static UIManager Instance { get; set; }

        private RectTransform modsListTab;
        private RectTransform modSettingsTab;
        private RectTransform mainMenu;
        private RectTransform mainMenuTitle;
        private RectTransform modWindowRect;
        private RectTransform modInfoWindowRect;

        public UIManager()
        {
            if (Instance == null)
                Instance = this;
            else
                Core.Logger.Warning("Duplicate UIManager Detected");
        }

        public void Initialize()
        {
            Core.Logger.Msg("Initializing UI...");
            CacheUIReferences();

            modsListTab = UIBuilder.CreateTab("Mods List", 700f, 500f);
            SetupModsListTab();

            modSettingsTab = UIBuilder.CreateTab("Mod Settings", 700f, 500f);
            SetupModSettingsTab();

            mainMenuTitle.localScale = new Vector3(0.9f, 0.9f, 0.9f);

            var modsButton = UIBuilder.CreateButton("Mods Button", mainMenu, "Mods", OpenModsMenu);
            modsButton.transform.SetSiblingIndex(2);
            Core.Logger.Msg("Created Mods Button.");
        }

        private void CacheUIReferences()
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Core.Logger.Error("UIManager: Canvas not found in scene.");
                return;
            }
            var margins = canvas.transform.Find("Margins");
            if (margins == null)
            {
                Core.Logger.Error("UIManager: Canvas/Margins not found.");
                return;
            }
            var menu = margins.Find("Main Menu");
            if (menu == null)
            {
                Core.Logger.Error("UIManager: Main Menu not found under Canvas/Margins.");
                return;
            }
            mainMenu = menu.GetComponent<RectTransform>();
            mainMenuTitle = mainMenu.Find("Title").GetComponent<RectTransform>();
        }

        private void SetupModsListTab()
        {
            var windowContent = UIBuilder.CreateHorizontalLayoutArea("Window Content", modsListTab, 100f, controlChildSize: false);
            modWindowRect = UIBuilder.CreateWindow("Mods List Window", windowContent, "Installed Mods", out TextMeshProUGUI titleText1, 500f, OpenMainMenu);

            modInfoWindowRect = UIBuilder.CreateWindow("Mod Info Window", windowContent, "Mod Info", out TextMeshProUGUI titleText2, 500f, null);
            var content = UIBuilder.CreateVerticalLayoutArea("Content", modInfoWindowRect, 10f, null, TextAnchor.MiddleCenter, true, true);
            UIBuilder.CreateText("Mod Name", content, "Name: N/A", Color.yellow, out TextMeshProUGUI text, 40);
            text.alignment = TextAlignmentOptions.Center;
            UIBuilder.CreateText("Mod Author", content, "Author: N/A", Color.yellow, out TextMeshProUGUI text2, 40);
            text2.alignment = TextAlignmentOptions.Center;
            UIBuilder.CreateText("Mod Version", content, "Version: N/A", Color.yellow, out TextMeshProUGUI text3, 40);
            text3.alignment = TextAlignmentOptions.Center;
            var settingsButton = UIBuilder.CreateButton("Settings Button", content, "Settings", null).GetComponent<UnityEngine.UI.Button>();

            var mid = modInfoWindowRect.gameObject.AddComponent<ModInfoDisplayer>();
            mid.modName = text;
            mid.modAuthor = text2;
            mid.modVersion = text3;
            mid.settingsButton = settingsButton;
        }

        private void SetupModSettingsTab()
        {
            var settingsWindow = UIBuilder.CreateWindow("Settings Window", modSettingsTab, "Mod Settings", out TextMeshProUGUI titleText, 1000f, OpenModsMenu);
            var content = UIBuilder.CreateTab("Content", settingsWindow, 700f, 500f);
            UIBuilder.CreateVerticalLayoutArea("Settings Container", content, 30, null, TextAnchor.UpperCenter, false, false);
            var settigsManager = content.gameObject.AddComponent<ModSettingsManager>();
            settigsManager.titleText = titleText;
            settigsManager.Init();
        }

        public void AddModButton(ModEntry mod)
        {
            var modButton = UIBuilder.CreateButton("Mod Button", modWindowRect, mod.Name, null);
            var meb = modButton.gameObject.AddComponent<ModEntryButton>();
            meb.mod = mod;
        }

        public void OpenSettings()
        {
            MainMenuManager.instance.OpenTab(modSettingsTab.gameObject);
        }

        public void OpenMainMenu()
        {
            MainMenuManager.instance.OpenTab(mainMenu.gameObject);
            ModInfoDisplayer.Instance.HandleClose();
        }

        public void OpenModsMenu()
        {
            MainMenuManager.instance.OpenTab(modsListTab.gameObject);
        }
    }
}
