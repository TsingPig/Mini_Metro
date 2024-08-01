using Michsky.MUIP;
using System.Threading.Tasks;
using TsingPigSDK;
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
            A();

        });
    }

    private async void A()
    {
        GameObject ripple = await Instantiater.InstantiateAsync(StrDef.Ripple, cityNodeButton.GetComponent<RectTransform>().transform);
        rippleEffect = ripple.GetComponent<RippleEffect>();
        rippleEffect.PlayRipple(new Vector2(cityNodeButton.transform.position.x,
                                cityNodeButton.transform.position.y));
    }
}
