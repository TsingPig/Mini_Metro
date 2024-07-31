using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [ExecuteInEditMode]
    public class UIManagerDropdownItem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private UIManager UIManagerAsset;

        public bool overrideColors = false;
        public bool overrideFonts = false;

        [Header("Resources")]
        [SerializeField] private Image itemBackground;

        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemText;

        private void Awake()
        {
            if(UIManagerAsset == null) { UIManagerAsset = Resources.Load<UIManager>("MUIP Manager"); }

            this.enabled = true;

            if(UIManagerAsset.enableDynamicUpdate == false)
            {
                UpdateDropdown();
                this.enabled = false;
            }
        }

        private void Update()
        {
            if(UIManagerAsset == null) { return; }
            if(UIManagerAsset.enableDynamicUpdate == true) { UpdateDropdown(); }
        }

        private void UpdateDropdown()
        {
            if(overrideFonts == false && itemText != null) { itemText.font = UIManagerAsset.dropdownItemFont; }
            if(overrideColors == false)
            {
                if(itemBackground != null) { itemBackground.color = UIManagerAsset.dropdownItemBackgroundColor; }
                if(itemIcon != null) { itemIcon.color = UIManagerAsset.dropdownItemPrimaryColor; }
                if(itemText != null) { itemText.color = UIManagerAsset.dropdownItemPrimaryColor; }
            }
        }
    }
}