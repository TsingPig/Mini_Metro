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
    public Sprite[] cityNodeFillTextures;
    public Sprite[] cityNodeOutlineTextures;

    public string cityNodeName;
    public CityNodeType cityNodeType;
    public Button cityNodeButton;
    public GameObject ripplePrefab;

    private MetroLine _metroLine = null;
    private RippleEffect _rippleEffect;

    private bool isDragging = false;
    private bool isPointerOver = false;

    public Vector2 StartLocalPoint
    {
        get
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 startWorldPoint = rectTransform.position;
            Vector2 startLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, startWorldPoint, null, out startLocalPoint);
            return startLocalPoint;
        }
    }

    public Vector2 EndLocalPoint
    {
        get
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 endWorldPoint = rectTransform.position;
            Vector2 endLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, endWorldPoint, null, out endLocalPoint);
            return endLocalPoint;
        }
    }

    public MetroLine MetroLine { get => _metroLine; set => _metroLine = value; }

    private void Update()
    {
        HandleMouseEvents();
        if(isDragging)
        {
            OnDrag();
        }
    }

    private void HandleMouseEvents()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(isPointerOver)
            {
                OnPointerDown();
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(isDragging)
            {
                OnPointerUp();
            }
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
        {
            if(!isPointerOver)
            {
                OnPointerEnter();
                isPointerOver = true;
            }
        }
        else
        {
            if(isPointerOver)
            {
                OnPointerExit();
                isPointerOver = false;
            }
        }
    }

    private void OnPointerDown()
    {
        if(MetroLineManager.Instance.isDrag == false)
        {
            Debug.Log($"OnPointerDown: {gameObject.name}");
            StartRipple();
            MetroLineManager.Instance.CreateMetroLine();
            MetroLineManager.Instance.isDrag = true;
            MetroLineManager.Instance.CurrentMetroLine.cityNodes.Add(this);

            Vector2 endLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, Input.mousePosition, null, out endLocalPoint);
            MetroLineManager.Instance.DrawLine(StartLocalPoint);
            MetroLineManager.Instance.UpdateLineEnd(endLocalPoint);

            isDragging = true;
        }
    }

    private void OnPointerUp()
    {
        Debug.Log($"OnPointerUp: {gameObject.name}");
        MetroLineManager.Instance.isDrag = false;
        isDragging = false;
    }

    private void OnDrag()
    {
        Debug.Log($"OnDrag: {gameObject.name}");
        if(MetroLineManager.Instance.isDrag)
        {
            Vector2 endLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, Input.mousePosition, null, out endLocalPoint);
            MetroLineManager.Instance.UpdateLineEnd(endLocalPoint);
        }
    }

    private void OnPointerEnter()
    {
        Debug.Log($"OnPointerEnter: {gameObject.name}");
        if(MetroLineManager.Instance.isDrag)
        {
            if(!MetroLineManager.Instance.CurrentMetroLine.cityNodes.Contains(this))
            {
                StartRipple();
                MetroLineManager.Instance.CurrentMetroLine.cityNodes.Add(this);
                MetroLineManager.Instance.UpdateLineEnd(EndLocalPoint);
                MetroLineManager.Instance.DrawLine(StartLocalPoint);
            }
        }
    }

    private void OnPointerExit()
    {
        Debug.Log($"OnPointerExit: {gameObject.name}");
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
