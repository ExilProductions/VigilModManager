using UnityEngine;
using UnityEngine.UI;

namespace VMM.UI.Components
{
    internal class UILayoutRebuilder : MonoBehaviour
    {
        RectTransform parentRect;

        void OnEnable()
        {
            if(parentRect == null)
            {
                parentRect = transform.parent.GetComponent<RectTransform>();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
        }
    }
}
