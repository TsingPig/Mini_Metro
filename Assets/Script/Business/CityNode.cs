using TsingPigSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CityNodeType
{
    Circle = 0,
    Triangle = 1,
    RoundedRectangle = 2,
    Diamond = 3
}

public class CityNode : MonoBehaviour, IPointerDownHandler
{
    public Sprite[] cityNodeFillTextures;
    public Sprite[] cityNodeOutlineTextures;

    public string cityNodeName;
    public CityNodeType cityNodeType;
    public Button cityNodeButton;
    public GameObject ripplePrefab;


    public MetroLine MetroLine => _metroLine;

    private MetroLine _metroLine;
    private RippleEffect _rippleEffect;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartRipple();
    }


    void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = cityNodeFillTextures[(int)cityNodeType];
        transform.GetChild(1).GetComponent<Image>().sprite = cityNodeOutlineTextures[(int)cityNodeType];
    }

    private void Update()
    {

    }

    private async void StartRipple()
    {
        GameObject ripple = await Instantiater.InstantiateAsync(Str.Ripple, cityNodeButton.GetComponent<RectTransform>().transform);
        _rippleEffect = ripple.GetComponent<RippleEffect>();
        _rippleEffect.PlayRipple(new Vector2(cityNodeButton.transform.position.x,
                                cityNodeButton.transform.position.y));
    }

  
}