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

public class CityNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Sprite[] cityNodeFillTextures;
    public Sprite[] cityNodeOutlineTextures;

    public string cityNodeName;
    public CityNodeType cityNodeType;
    public Button cityNodeButton;
    public GameObject ripplePrefab;

    public MetroLine MetroLine { get => _metroLine; set => _metroLine = value; }

    private MetroLine _metroLine = null;
    private RippleEffect _rippleEffect;


    public void OnPointerDown(PointerEventData eventData)
    {
        StartRipple();
        MetroLineManager.Instance.CreateMetroLine();
        Debug.Log("OnPointerDown");

    }

    public async void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");

        RectTransform rectTransform = GetComponent<RectTransform>();

        // 获取起始坐标（世界坐标）
        Vector2 startWorldPoint = rectTransform.position;

        // 将世界坐标转换为RectTransform的局部坐标
        Vector2 startLocalPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, startWorldPoint, null, out startLocalPoint);

        // 将屏幕坐标转换为RectTransform的局部坐标
        Vector2 endLocalPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, eventData.position, eventData.pressEventCamera, out endLocalPoint);

        await MetroLineManager.Instance.DrawLineBetween(startLocalPoint, endLocalPoint, MetroLineManager.Instance.CurrentMetroLineRoot, MetroLineManager.Instance.CurrentMetroLineColor);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");


    }

    private void Start()
    {
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

    public void UpdateCityNodeImage()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = cityNodeFillTextures[(int)cityNodeType];
        transform.GetChild(1).GetComponent<Image>().sprite = cityNodeOutlineTextures[(int)cityNodeType];
    }


}