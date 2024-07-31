using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ModalWindowManager : MonoBehaviour
    {
        // Resources
        public Image windowIcon;

        public TextMeshProUGUI windowTitle;
        public TextMeshProUGUI windowDescription;
        public ButtonManager confirmButton;
        public ButtonManager cancelButton;
        public Animator mwAnimator;

        // Content
        public Sprite icon;

        public string titleText = "Title";
        [TextArea] public string descriptionText = "Description here";

        // Events
        public UnityEvent onOpen;

        public UnityEvent onConfirm;
        public UnityEvent onCancel;

        // Settings
        public bool useCustomContent = false;

        public bool isOn = false;
        public bool closeOnCancel = true;
        public bool closeOnConfirm = true;
        public bool showCancelButton = true;
        public bool showConfirmButton = true;
        public StartBehaviour startBehaviour = StartBehaviour.Disable;
        public CloseBehaviour closeBehaviour = CloseBehaviour.Disable;

        public enum StartBehaviour
        { None, Disable, Enable }

        public enum CloseBehaviour
        { None, Disable, Destroy }

        private void Awake()
        {
            isOn = false;

            if(mwAnimator == null) { mwAnimator = gameObject.GetComponent<Animator>(); }
            if(closeOnCancel == true) { onCancel.AddListener(CloseWindow); }
            if(closeOnConfirm == true) { onConfirm.AddListener(CloseWindow); }
            if(confirmButton != null) { confirmButton.onClick.AddListener(onConfirm.Invoke); }
            if(cancelButton != null) { cancelButton.onClick.AddListener(onCancel.Invoke); }
            if(startBehaviour == StartBehaviour.Disable) { isOn = false; gameObject.SetActive(false); }
            else if(startBehaviour == StartBehaviour.Enable) { isOn = false; OpenWindow(); }

            UpdateUI();
        }

        public void UpdateUI()
        {
            if(useCustomContent == true)
                return;

            if(windowIcon != null) { windowIcon.sprite = icon; }
            if(windowTitle != null) { windowTitle.text = titleText; }
            if(windowDescription != null) { windowDescription.text = descriptionText; }

            if(showCancelButton == true && cancelButton != null) { cancelButton.gameObject.SetActive(true); }
            else if(cancelButton != null) { cancelButton.gameObject.SetActive(false); }

            if(showConfirmButton == true && confirmButton != null) { confirmButton.gameObject.SetActive(true); }
            else if(confirmButton != null) { confirmButton.gameObject.SetActive(false); }
        }

        public void Open()
        {
            if(isOn == false)
            {
                StopCoroutine("DisableObject");

                gameObject.SetActive(true);
                isOn = true;
                onOpen.Invoke();
                mwAnimator.Play("Fade-in");
            }
        }

        public void Close()
        {
            if(isOn == true)
            {
                isOn = false;
                mwAnimator.Play("Fade-out");

                StartCoroutine("DisableObject");
            }
        }

        // Obsolete
        public void OpenWindow()
        { Open(); }

        public void CloseWindow()
        { Close(); }

        public void AnimateWindow()
        {
            if(isOn == false)
            {
                StopCoroutine("DisableObject");

                isOn = true;
                gameObject.SetActive(true);
                mwAnimator.Play("Fade-in");
            }
            else
            {
                isOn = false;
                mwAnimator.Play("Fade-out");

                StartCoroutine("DisableObject");
            }
        }

        private IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1);

            if(closeBehaviour == CloseBehaviour.Disable) { gameObject.SetActive(false); }
            else if(closeBehaviour == CloseBehaviour.Destroy) { Destroy(gameObject); }
        }
    }
}