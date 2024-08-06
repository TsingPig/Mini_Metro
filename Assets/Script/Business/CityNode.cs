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

    private GameObject _currentLineObj;
    private RectTransform _lineRoot;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartRipple();
        if(_metroLine == null)
        {
            // 创建线路模式

            MetroLineManager.Instance.CreateMetroLine();

            _lineRoot = GetComponent<RectTransform>();
            _currentLineObj = Instantiate(MetroLineManager.Instance.linePrefab, _lineRoot);
            _currentLineObj.GetComponent<Image>().color = MetroLineManager.Instance.CurrentMetroLineColor;

            RectTransform lineRectTransform = _currentLineObj.GetComponent<RectTransform>();
            lineRectTransform.sizeDelta = Vector2.zero;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _lineRoot,
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
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _lineRoot,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            Vector2 startPos = _lineRoot.anchoredPosition;
            Vector2 endPos = localPoint;

            // 更新线条的大小和位置
            Vector2 sizeDelta = new Vector2(Vector2.Distance(startPos, endPos), Const.metroLineWidth);
            _lineRoot.sizeDelta = sizeDelta;
            _lineRoot.pivot = new Vector2(0, 0.5f);
            _lineRoot.anchoredPosition = startPos;

            // 计算旋转角度
            float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
            _lineRoot.localRotation = Quaternion.Euler(0, 0, angle);
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