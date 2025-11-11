using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VMM.UI.Components;

namespace VMM.UI.Helpers
{
    internal static class UIBuilder
    {
        const string TABS_CONTAINER_PATH = "Canvas/Margins";
        const string BUTTON_PATH = TABS_CONTAINER_PATH + "/Main Menu/Play Button";
        const string OPTIONS_PATH = TABS_CONTAINER_PATH + "/Options";
        const string DIVIDER_PATH = OPTIONS_PATH + "/Divider";
        const string SLIDER_PATH = OPTIONS_PATH + "/Master Volume";

        public static RectTransform CreateTab(string name, float x, float y)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var tabObj = new GameObject(name, typeof(RectTransform));
            tabObj.transform.SetParent(GameObject.Find(TABS_CONTAINER_PATH).transform, false);
            tabObj.layer = LayerMask.NameToLayer("UI");
            tabObj.SetActive(false);
            var tabRect = tabObj.GetComponent<RectTransform>();
            tabRect.sizeDelta = new Vector2(x, y);

            return tabRect;
        }

        public static RectTransform CreateTab(string name, RectTransform parent, float x, float y)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var tabObj = new GameObject(name, typeof(RectTransform));
            tabObj.transform.SetParent(parent, false);
            tabObj.layer = LayerMask.NameToLayer("UI");
            var tabRect = tabObj.GetComponent<RectTransform>();
            tabRect.sizeDelta = new Vector2(x, y);

            return tabRect;
        }

        public static RectTransform CreateButton(string name, RectTransform parent, string buttonText, UnityAction callback)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var newButton = UnityEngine.Object.Instantiate(GameObject.Find(BUTTON_PATH), parent);
            newButton.name = name;
            var buttonComp = newButton.GetComponent<UnityEngine.UI.Button>();
            buttonComp.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            if (callback != null)
                buttonComp.onClick.AddListener(callback);
            var selectedText = newButton.transform.Find("Selected Text").GetComponent<TextMeshProUGUI>();
            selectedText.text = $"[ {buttonText} ] ";
            var unselectedText = newButton.transform.Find("Unselected Text").GetComponent<TextMeshProUGUI>();
            unselectedText.text = buttonText;
            newButton.AddComponent<UILayoutRebuilder>();

            return newButton.GetComponent<RectTransform>();
        }

        public static RectTransform CreateText(string name, RectTransform parent, string text, Color color, out TextMeshProUGUI textMesh, int fontSize = 14)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                textMesh = null;
                return null;
            }

            var newText = UnityEngine.Object.Instantiate(GameObject.Find(BUTTON_PATH + "/Unselected Text"), parent);
            newText.name = name;
            UnityEngine.Object.DestroyImmediate(newText.GetComponent<ContentSizeFitter>());
            var textComp = newText.GetComponent<TextMeshProUGUI>();
            textComp.text = text;
            if(color != Color.yellow)
                textComp.color = color;
            textComp.fontSize = fontSize;
            textMesh = textComp;
            newText.AddComponent<UILayoutRebuilder>();

            return newText.GetComponent<RectTransform>();
        }

        public static RectTransform CreateWindow(string name, RectTransform parent, string title, out TextMeshProUGUI titleText, float width, UnityAction onClose)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                titleText = null;
                return null;
            }

            var newWindow = UnityEngine.Object.Instantiate(GameObject.Find(OPTIONS_PATH), parent);
            newWindow.name = name;
            for (int i = 0; i < newWindow.transform.childCount; i++)
            {
                var child = newWindow.transform.GetChild(i);
                if(child.name != "Header")
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
            var text = newWindow.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = title;
            titleText = text;
            var closeButton = newWindow.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Button>();
            closeButton.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            if(onClose != null)
                closeButton.onClick.AddListener(onClose);
            else 
                closeButton.gameObject.SetActive(false);
            var headerRect = newWindow.transform.GetChild(0).GetComponent<RectTransform>();
            headerRect.sizeDelta = new Vector2(width, headerRect.sizeDelta.y);
            var closeContainer = headerRect.GetChild(1).GetComponent<RectTransform>();
            closeContainer.anchorMin = new Vector2(1, 0.5f);
            closeContainer.anchorMax = new Vector2(1, 0.5f);
            closeContainer.pivot = new Vector2(1, 0.5f);
            closeContainer.anchoredPosition = new Vector2(10f, 10f);
            var buttonRect = closeContainer.GetChild(0).GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = Vector2.zero;
            var windowRect = newWindow.GetComponent<RectTransform>();
            if (newWindow.activeSelf == false)
                newWindow.SetActive(true);
            newWindow.AddComponent<UILayoutRebuilder>();

            return windowRect;
        }

        public static RectTransform CreateDivider(string name, RectTransform parent, Color color)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var newDivider = UnityEngine.Object.Instantiate(GameObject.Find(DIVIDER_PATH), parent);
            newDivider.name = name;
            var textComp = newDivider.GetComponent<TextMeshProUGUI>();
            StringBuilder stringBuilder = new StringBuilder();
            float parentWidth = parent.sizeDelta.x;
            int dashCount = Mathf.FloorToInt(parentWidth / 10f);
            for (int i = 0; i < dashCount; i++)
            {
                stringBuilder.Append("-");
            }
            textComp.text = stringBuilder.ToString();
            textComp.color = color;
            var rect = newDivider.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, rect.anchorMin.y);
            rect.anchorMax = new Vector2(1f, rect.anchorMax.y);
            rect.offsetMin = new Vector2(0f, rect.offsetMin.y);
            rect.offsetMax = new Vector2(0f, rect.offsetMax.y);
            newDivider.AddComponent<UILayoutRebuilder>();

            return rect;
        }

        public static RectTransform CreateVerticalLayoutArea(string name, RectTransform parent, float spacing = 8f, RectOffset padding = null, TextAnchor alignment = TextAnchor.MiddleCenter, bool controlChildSize = true, bool forceExpand = false)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var layoutObj = new GameObject(name, typeof(RectTransform));
            layoutObj.transform.SetParent(parent, false);
            layoutObj.layer = LayerMask.NameToLayer("UI");
            var rect = layoutObj.GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.zero;
            var layout = layoutObj.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = alignment;
            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset(0, 0, 0, 0);
            layout.childControlWidth = controlChildSize;
            layout.childControlHeight = controlChildSize;
            layout.childForceExpandWidth = forceExpand;
            layout.childForceExpandHeight = forceExpand;
            var fitter = layoutObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            layoutObj.AddComponent<UILayoutRebuilder>();

            return rect;
        }

        public static RectTransform CreateHorizontalLayoutArea(string name, RectTransform parent, float spacing = 8f, RectOffset padding = null, TextAnchor alignment = TextAnchor.MiddleCenter, bool controlChildSize = true, bool forceExpand = false)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var layoutObj = new GameObject(name, typeof(RectTransform));
            layoutObj.transform.SetParent(parent, false);
            layoutObj.layer = LayerMask.NameToLayer("UI");
            var rect = layoutObj.GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.zero;
            var layout = layoutObj.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = alignment;
            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset(0, 0, 0, 0);
            layout.childControlWidth = controlChildSize;
            layout.childControlHeight = controlChildSize;
            layout.childForceExpandWidth = forceExpand;
            layout.childForceExpandHeight = forceExpand;
            var fitter = layoutObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            layoutObj.AddComponent<UILayoutRebuilder>();

            return rect;
        }

        public static RectTransform CreateSlider(string name, RectTransform parent, string sliderText, float maxValue, float minValue, float value, UnityAction<float> onValueChanged)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var newSlider = UnityEngine.Object.Instantiate(GameObject.Find(SLIDER_PATH), parent);
            newSlider.name = name;
            UnityEngine.Object.Destroy(newSlider.GetComponent<UIVolumeSlider>());
            newSlider.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sliderText;
            var sliderComp = newSlider.transform.GetChild(2).GetComponent<Slider>();
            sliderComp.minValue = minValue;
            sliderComp.maxValue = maxValue;
            sliderComp.value = value;
            sliderComp.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            sliderComp.onValueChanged.AddListener(onValueChanged);
            var rectComp = newSlider.GetComponent<RectTransform>();
            newSlider.AddComponent<UILayoutRebuilder>();

            return rectComp;
        }

        public static RectTransform CreateToggle(string name, RectTransform parent, string toggleText, bool value, UnityAction<bool> onValueChanged)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                return null;
            }

            var rootToggle = CreateHorizontalLayoutArea(name, parent, 15, null, TextAnchor.MiddleLeft, false, true);
            var toggleContainer = new GameObject("ToggleContainer", typeof(RectTransform));
            toggleContainer.transform.SetParent(rootToggle.transform, false);
            toggleContainer.layer = LayerMask.NameToLayer("UI");
            var toggleContainerRect = toggleContainer.GetComponent<RectTransform>();
            toggleContainerRect.sizeDelta = new Vector2(40f, 40f);
            var toggleShadow = new GameObject("ToggleShadow", typeof(RectTransform));
            toggleShadow.transform.SetParent(toggleContainer.transform, false);
            toggleShadow.layer = LayerMask.NameToLayer("UI");
            var toggleShadowRect = toggleShadow.GetComponent<RectTransform>();
            toggleShadowRect.sizeDelta = new Vector2(40f, 40f);
            var toggleShadowImage = toggleShadow.AddComponent<Image>();
            toggleShadowImage.color = Color.black;
            var toggleGraphic = new GameObject("ToggleGraphic", typeof(RectTransform));
            toggleGraphic.transform.SetParent(toggleShadow.transform, false);
            toggleGraphic.layer = LayerMask.NameToLayer("UI");
            var toggleGraphicRect = toggleGraphic.GetComponent<RectTransform>();
            toggleGraphicRect.sizeDelta = new Vector2(30f, 30f);
            var toggleGraphicImage = toggleGraphic.AddComponent<Image>();
            toggleGraphicImage.color = Color.yellow;
            var toggleComp = toggleContainer.AddComponent<Toggle>();
            toggleComp.transition = Selectable.Transition.None;
            toggleComp.targetGraphic = toggleShadowImage;
            toggleComp.graphic = toggleGraphicImage;
            toggleComp.isOn = value;
            toggleComp.onValueChanged.AddListener(onValueChanged);
            CreateText("ToggleText", rootToggle, toggleText, Color.yellow, out TextMeshProUGUI text, 28);
            text.alignment = TextAlignmentOptions.BaselineLeft;

            return rootToggle;
        }

        public static RectTransform CreateVerticalScrollView(string name, RectTransform parent, float width, float height, out RectTransform contentRect, float contentPadding = 8f)
        {
            if (!InMainMenu())
            {
                Core.Logger.Warning("Tried Creating UI outside Main Menu, Aborting.");
                contentRect = null;
                return null;
            }

            // Root ScrollView object
            var scrollViewObj = new GameObject(name, typeof(RectTransform), typeof(ScrollRect), typeof(Image));
            scrollViewObj.transform.SetParent(parent, false);
            scrollViewObj.layer = LayerMask.NameToLayer("UI");

            var scrollViewRect = scrollViewObj.GetComponent<RectTransform>();
            scrollViewRect.sizeDelta = new Vector2(width, height);

            // Make background invisible
            var scrollViewImage = scrollViewObj.GetComponent<Image>();
            scrollViewImage.color = Color.clear;

            // Viewport
            var viewportObj = new GameObject("Viewport", typeof(RectTransform), typeof(UnityEngine.UI.Mask), typeof(Image));
            viewportObj.transform.SetParent(scrollViewObj.transform, false);
            viewportObj.layer = LayerMask.NameToLayer("UI");

            var viewportRect = viewportObj.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;

            var viewportImage = viewportObj.GetComponent<Image>();
            viewportImage.color = Color.clear;

            var mask = viewportObj.GetComponent<UnityEngine.UI.Mask>();
            mask.showMaskGraphic = false;

            // Content container
            var contentObj = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            contentObj.transform.SetParent(viewportObj.transform, false);
            contentObj.layer = LayerMask.NameToLayer("UI");

            var contentRectComp = contentObj.GetComponent<RectTransform>();
            contentRectComp.anchorMin = new Vector2(0, 1);
            contentRectComp.anchorMax = new Vector2(1, 1);
            contentRectComp.pivot = new Vector2(0.5f, 1);
            contentRectComp.anchoredPosition = Vector2.zero;
            contentRectComp.sizeDelta = new Vector2(0, 0);

            var layout = contentObj.GetComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.spacing = contentPadding;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            var fitter = contentObj.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            // ScrollRect setup
            var scrollRect = scrollViewObj.GetComponent<ScrollRect>();
            scrollRect.content = contentRectComp;
            scrollRect.viewport = viewportRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

            // Scrollbar
            var scrollbarObj = new GameObject("Scrollbar", typeof(RectTransform), typeof(Scrollbar), typeof(Image));
            scrollbarObj.transform.SetParent(scrollViewObj.transform, false);
            scrollbarObj.layer = LayerMask.NameToLayer("UI");

            var scrollbarRect = scrollbarObj.GetComponent<RectTransform>();
            scrollbarRect.anchorMin = new Vector2(1, 0);
            scrollbarRect.anchorMax = new Vector2(1, 1);
            scrollbarRect.pivot = new Vector2(1, 1);
            scrollbarRect.sizeDelta = new Vector2(20f, 0);
            scrollbarRect.anchoredPosition = Vector2.zero;

            var scrollbarImage = scrollbarObj.GetComponent<Image>();
            scrollbarImage.color = Color.yellow;

            var scrollbar = scrollbarObj.GetComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;

            scrollRect.verticalScrollbar = scrollbar;

            scrollViewObj.AddComponent<UILayoutRebuilder>();

            contentRect = contentRectComp;

            return scrollViewRect;
        }


        static bool InMainMenu()
        {
            return SceneManager.GetActiveScene().buildIndex == 0;
        }
    }
}
