using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [ExecuteInEditMode]
    public class UIManagerProgressBar : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private UIManager UIManagerAsset;

        [HideInInspector] public bool overrideColors = false;
        [HideInInspector] public bool overrideFonts = false;

        [Header("Resources")]
        [SerializeField] private Image bar;

        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI label;

        private bool dynamicUpdateEnabled;

        private void Awake()
        {
            if(UIManagerAsset == null) { UIManagerAsset = Resources.Load<UIManager>("MUIP Manager"); }

            this.enabled = true;

            if(UIManagerAsset.enableDynamicUpdate == false)
            {
                UpdateProgressBar();
                this.enabled = false;
            }
        }

        private void Update()
        {
            if(UIManagerAsset == null) { return; }
            if(UIManagerAsset.enableDynamicUpdate == true) { UpdateProgressBar(); }
        }

        private void UpdateProgressBar()
        {
            if(overrideColors == false)
            {
                bar.color = UIManagerAsset.progressBarColor;
                background.color = UIManagerAsset.progressBarBackgroundColor;
                label.color = UIManagerAsset.progressBarLabelColor;
            }

            if(overrideFonts == false)
            {
                label.font = UIManagerAsset.progressBarLabelFont;
                label.fontSize = UIManagerAsset.progressBarLabelFontSize;
            }
        }
    }
}