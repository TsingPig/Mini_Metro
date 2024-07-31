using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [ExecuteInEditMode]
    public class UIManagerTooltip : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private UIManager UIManagerAsset;

        [Header("Resources")]
        [SerializeField] private Image background;

        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            if(UIManagerAsset == null) { UIManagerAsset = Resources.Load<UIManager>("MUIP Manager"); }

            this.enabled = true;

            if(UIManagerAsset.enableDynamicUpdate == false)
            {
                UpdateTooltip();
                this.enabled = false;
            }
        }

        private void Update()
        {
            if(UIManagerAsset == null) { return; }
            if(UIManagerAsset.enableDynamicUpdate == true) { UpdateTooltip(); }
        }

        private void UpdateTooltip()
        {
            background.color = UIManagerAsset.tooltipBackgroundColor;
            text.color = UIManagerAsset.tooltipTextColor;
            text.font = UIManagerAsset.tooltipFont;
            text.fontSize = UIManagerAsset.tooltipFontSize;
        }
    }
}