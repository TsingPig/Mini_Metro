using UnityEngine;
using UnityEngine.UI;

public enum CityNodeType
{
    Circle = 0,
    Triangle = 1,
    RoundedRectangle = 2,
    Diamond = 3
}

public class CityNode : MonoBehaviour
{
    public CityNodeType cityNodeType;
    public string cityNodeName;
    public Button cityNodeButton;
    public GameObject ripplePrefab;

    private MetroLine _metroLine;
    private RippleEffect rippleEffect;

    public MetroLine MetroLine => _metroLine;

    void Start()
    {


        cityNodeButton.onClick.AddListener(() =>
        {
            GameObject ripple = Instantiate(ripplePrefab, transform);
            rippleEffect = ripple.GetComponent<RippleEffect>();
            Vector2 clickPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(cityNodeButton.GetComponent<RectTransform>(), Input.mousePosition, null, out clickPosition);
            rippleEffect.PlayRipple(cityNodeButton.transform.TransformPoint(clickPosition));
        });
    }
}
