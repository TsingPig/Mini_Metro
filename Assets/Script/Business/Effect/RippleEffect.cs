using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class RippleEffect : MonoBehaviour
{
    public float rippleDuration = 0.5f;
    public float maxRippleRadius = 100f;
    public Color rippleColor = new Color(1f, 1f, 1f, 0.5f);

    private Image rippleImage;
    private RectTransform rippleTransform;
    private float elapsedTime;

    private void Awake()
    {
        rippleImage = gameObject.GetComponent<Image>();
        rippleImage.color = rippleColor;
        rippleTransform = GetComponent<RectTransform>();
        rippleTransform.localScale = Vector3.zero;
    }

    public void PlayRipple(Vector2 position)
    {
        rippleTransform.position = position;
        elapsedTime = 0f;
        rippleTransform.localScale = Vector3.zero;
        rippleImage.enabled = true;
    }

    private void Update()
    {
        if(elapsedTime < rippleDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(0f, maxRippleRadius, elapsedTime / rippleDuration);
            rippleTransform.localScale = new Vector3(scale, scale, scale);

            float alpha = Mathf.Lerp(rippleColor.a, 0f, elapsedTime / rippleDuration);
            rippleImage.color = new Color(rippleColor.r, rippleColor.g, rippleColor.b, alpha);
        }
        else
        {
            rippleImage.enabled = false;
            Instantiater.DeactivateObject(gameObject);
        }
    }
}