using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(Animator))]
    public class CustomToggle : MonoBehaviour
    {
        [HideInInspector] public Toggle toggleObject;
        [HideInInspector] public Animator toggleAnimator;

        [Header("Settings")]
        public bool invokeOnAwake;

        private bool isInitialized = false;

        private void Awake()
        {
            if(toggleObject == null) { toggleObject = gameObject.GetComponent<Toggle>(); }
            if(toggleAnimator == null) { toggleAnimator = toggleObject.GetComponent<Animator>(); }
            if(invokeOnAwake == true) { toggleObject.onValueChanged.Invoke(toggleObject.isOn); }

            toggleObject.onValueChanged.AddListener(UpdateState);
            UpdateState();
            isInitialized = true;
        }

        private void OnEnable()
        {
            if(isInitialized == false)
                return;

            UpdateState();
        }

        public void UpdateState()
        {
            if(gameObject.activeInHierarchy == true)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            toggleAnimator.enabled = true;

            if(toggleObject.isOn) { toggleAnimator.Play("On Instant"); }
            else { toggleAnimator.Play("Off Instant"); }
        }

        public void UpdateState(bool value)
        {
            if(gameObject.activeInHierarchy == true)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            toggleAnimator.enabled = true;

            if(toggleObject.isOn) { toggleAnimator.Play("Toggle On"); }
            else { toggleAnimator.Play("Toggle Off"); }
        }

        private IEnumerator DisableAnimator()
        {
            yield return new WaitForSecondsRealtime(0.6f);
            toggleAnimator.enabled = false;
        }
    }
}