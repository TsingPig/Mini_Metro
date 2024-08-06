using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum CityNodeType
{
    Circle = 0,
    Triangle = 1,
    RoundedRectangle = 2,
    Diamond = 3
}

public class CityNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler
{
    public Sprite[] cityNodeFillTextures;
    public Sprite[] cityNodeOutlineTextures;

    public string cityNodeName;
    public CityNodeType cityNodeType;
    public Button cityNodeButton;
    public GameObject ripplePrefab;

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

    private MetroLine _metroLine = null;
    private RippleEffect _rippleEffect;

    public async void OnPointerDown(PointerEventData eventData)
    {
        if(MetroLineManager.Instance.isDrag == false)
        {
            Debug.Log("OnPointerDown");
            StartRipple();
            MetroLineManager.Instance.CreateMetroLine();
            MetroLineManager.Instance.isDrag = true;
            MetroLineManager.Instance.CurrentMetroLine.cityNodes.Add(this);

            Vector2 endLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, eventData.position, eventData.pressEventCamera, out endLocalPoint);
            await MetroLineManager.Instance.DrawLine(StartLocalPoint);
            MetroLineManager.Instance.UpdateLineEnd(endLocalPoint);

        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        MetroLineManager.Instance.isDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if(MetroLineManager.Instance.isDrag)
        {
            Vector2 endLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MetroLineManager.Instance.CurrentMetroLineRoot, eventData.position, eventData.pressEventCamera, out endLocalPoint);
            MetroLineManager.Instance.UpdateLineEnd(endLocalPoint);
        }
    }

    public async void OnPointerEnter(PointerEventData eventData)
    {
        if(MetroLineManager.Instance.isDrag)
        {
            if(!MetroLineManager.Instance.CurrentMetroLine.cityNodes.Contains(this))
            {
                MetroLineManager.Instance.isDrag = false;
                StartRipple();
                MetroLineManager.Instance.CurrentMetroLine.cityNodes.Add(this);
                MetroLineManager.Instance.UpdateLineEnd(EndLocalPoint);
                
                await MetroLineManager.Instance.DrawLine(StartLocalPoint);
                Debug.Log("OnPointerEnter");

                MetroLineManager.Instance.isDrag = true;
            }
        }
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