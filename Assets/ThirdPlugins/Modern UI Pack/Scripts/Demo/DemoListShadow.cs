using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    public class DemoListShadow : MonoBehaviour, IBeginDragHandler
    {
        [Header("Resources")]
        [SerializeField] private Scrollbar listScrollbar;

        [SerializeField] private CanvasGroup leftCG;
        [SerializeField] private CanvasGroup rightCG;

        [Header("Settings")]
        [SerializeField] private float scrollTime = 5;

        [SerializeField] private float transitionSpeed = 4;

        private void Awake()
        {
            CheckForValue(0);
        }

        public void CheckForValue(float value)
        {
            if(value > 0.05)
            {
                StopCoroutine("LeftCGFadeOut");
                StartCoroutine("LeftCGFadeIn");
            }
            else
            {
                StopCoroutine("LeftCGFadeIn");
                StartCoroutine("LeftCGFadeOut");
            }

            if(value < 0.95)
            {
                StopCoroutine("RightCGFadeOut");
                StartCoroutine("RightCGFadeIn");
            }
            else
            {
                StopCoroutine("RightCGFadeIn");
                StartCoroutine("RightCGFadeOut");
            }
        }

        public void ScrollUp()
        { StopCoroutine("ScrollDownHelper"); StartCoroutine("ScrollUpHelper"); }

        public void ScrollDown()
        { StopCoroutine("ScrollUpHelper"); StartCoroutine("ScrollDownHelper"); }

        public void OnBeginDrag(PointerEventData data)
        { StopCoroutine("ScrollUpHelper"); StopCoroutine("ScrollDownHelper"); }

        private IEnumerator ScrollUpHelper()
        {
            float elapsedTime = 0;

            while(elapsedTime < scrollTime)
            {
                listScrollbar.value = Mathf.Lerp(listScrollbar.value, 0, elapsedTime / scrollTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ScrollDownHelper()
        {
            float elapsedTime = 0;

            while(elapsedTime < scrollTime)
            {
                listScrollbar.value = Mathf.Lerp(listScrollbar.value, 1, elapsedTime / scrollTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator LeftCGFadeIn()
        {
            leftCG.interactable = true;
            leftCG.blocksRaycasts = true;

            while(leftCG.alpha < 0.99f)
            {
                leftCG.alpha += Time.unscaledDeltaTime * transitionSpeed;
                yield return new WaitForEndOfFrame();
            }

            leftCG.alpha = 1;
        }

        private IEnumerator LeftCGFadeOut()
        {
            leftCG.interactable = false;
            leftCG.blocksRaycasts = false;

            while(leftCG.alpha > 0.01f)
            {
                leftCG.alpha -= Time.unscaledDeltaTime * transitionSpeed;
                yield return new WaitForEndOfFrame();
            }

            leftCG.alpha = 0;
        }

        private IEnumerator RightCGFadeIn()
        {
            rightCG.interactable = true;
            rightCG.blocksRaycasts = true;

            while(rightCG.alpha < 0.99f)
            {
                rightCG.alpha += Time.unscaledDeltaTime * transitionSpeed;
                yield return new WaitForEndOfFrame();
            }

            rightCG.alpha = 1;
        }

        private IEnumerator RightCGFadeOut()
        {
            rightCG.interactable = false;
            rightCG.blocksRaycasts = false;

            while(rightCG.alpha > 0.01f)
            {
                rightCG.alpha -= Time.unscaledDeltaTime * transitionSpeed;
                yield return new WaitForEndOfFrame();
            }

            rightCG.alpha = 0;
        }
    }
}