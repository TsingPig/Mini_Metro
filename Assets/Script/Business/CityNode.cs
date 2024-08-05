using TsingPigSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
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

    public MetroLine MetroLine { get => _metroLine; set => _metroLine = value; }

    private MetroLine _metroLine = null;
    private RippleEffect _rippleEffect;

    private GameObject _currentLineObj;
    private RectTransform _metroLineRoot;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartRipple();
        if(_metroLine == null)
        {
            // 创建线路模式


            _metroLineRoot = MetroLineManager.Instance.CurrentMetroLineRoot;
            _currentLineObj = Instantiate(MetroLineManager.Instance.linePrefab, _metroLineRoot);
            _currentLineObj.GetComponent<Image>().color = MetroLineManager.Instance.CurrentMetroLineColor;

            RectTransform lineRectTransform = _currentLineObj.GetComponent<RectTransform>();
            lineRectTransform.sizeDelta = Vector2.zero;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _metroLineRoot,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);
            lineRectTransform.anchoredPosition = localPoint;
        }
        else
        {
            // 显示所属线路

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentLineObj = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if(_metroLineRoot != null)
        //{
        //    Vector2 localPoint;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //        lineRoot,
        //        eventData.position,
        //        eventData.pressEventCamera,
        //        out localPoint);

        //    Vector2 startPos = _metroLineRoot.anchoredPosition;
        //    Vector2 endPos = localPoint;

        //    // 更新线条的大小和位置
        //    Vector2 sizeDelta = new Vector2(Vector2.Distance(startPos, endPos), lineWidth);
        //    _metroLineRoot.sizeDelta = sizeDelta;
        //    _metroLineRoot.pivot = new Vector2(0, 0.5f);
        //    _metroLineRoot.anchoredPosition = startPos;

        //    // 计算旋转角度
        //    float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
        //    _metroLineRoot.localRotation = Quaternion.Euler(0, 0, angle);
        //}
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