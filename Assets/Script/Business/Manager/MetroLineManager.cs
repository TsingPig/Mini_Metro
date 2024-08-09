using Sirenix.OdinInspector;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MetroLineManager : Singleton<MetroLineManager>
{
    public RectTransform metroLineRoot;

    public List<MetroLine> metroLines;

    public Vector2 StartLocalPoint
    {
        get { return _startLocalPoint; }
        set { _startLocalPoint = value; }
    }

    public Vector2 EndLocalPoint
    {
        get { return _endLocalPoint; }
        set { _endLocalPoint = value; }
    }

    public bool isDrag = false;

    [HideInInspector]
    public List<Color> metroLineColors;

    public Color CurrentMetroLineColor
    {
        get
        {
            if(MetroLineCount > 0)
            {
                return metroLineColors[MetroLineCount];
            }
            else
            {
                Debug.LogError("MetroLineCount is Zero");
                return Color.black;
            }
        }
    }

    public RectTransform CurrentMetroLineRoot
    {
        get
        {
            if(MetroLineCount > 0)
            {
                return metroLineRoot.GetChild(MetroLineCount - 1).transform.GetChild(0).GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("MetroLineCount is Zero");
                return null;
            }
        }
    }

    public MetroLine CurrentMetroLine
    {
        get
        {
            if(MetroLineCount > 0)
            {
                return metroLines[metroLines.Count - 1];
            }
            else
            {
                Debug.LogError("MetroLineCount is Zero");
                return null;
            }
        }
    }

    public GameObject CurrentLineObj
    {
        get { return _currentLineObj; }
    }

    /// <summary>
    /// ����Canvas��ָ��ָ�ŵ�����
    /// </summary>
    public GameObject GetUIMouseObj
    {
        get
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            _graphicRaycaster.Raycast(pointerEventData, raycastResults);
            foreach(var raycastResult in raycastResults)
            {
                GameObject obj = raycastResult.gameObject;
                Debug.Log(obj);
                if(obj.CompareTag(Const.CheckTargetTag))
                {
                    return obj;
                }
            }

            return null;
        }
    }

    private int MetroLineCount
    {
        get { return metroLines.Count; }
    }

    private GameObject _currentLineObj;
    private Vector2 _startLocalPoint;
    private Vector2 _endLocalPoint;
    private GraphicRaycaster _graphicRaycaster;

    /// <summary>
    /// �����µĵ�����·
    /// </summary>
    /// <returns></returns>
    public MetroLine CreateMetroLine()
    {
        GameObject metroLineObj = Instantiater.Instantiate(Str.METRO_LINE_DATA_PATH, metroLineRoot, $"MetroLine{metroLines.Count}");
        MetroLine metroLine = metroLineObj.GetComponent<MetroLine>();
        metroLines.Add(metroLine);
        return metroLine;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="startLocalPoint"></param>
    /// <returns></returns>
    public void DrawLine(Vector2 startLocalPoint)
    {
        RectTransform lineRoot = CurrentMetroLineRoot;
        Color color = CurrentMetroLineColor;

        _currentLineObj = Instantiater.Instantiate(Str.LINE_PREFAB_DATA_PATH, lineRoot);
        _currentLineObj.GetComponent<Image>().color = color;

        RectTransform lineRectTransform = _currentLineObj.GetComponent<RectTransform>();
        lineRectTransform.sizeDelta = Vector2.zero;

        _startLocalPoint = startLocalPoint;
        Vector2 sizeDelta = new Vector2(Vector2.Distance(_startLocalPoint, _endLocalPoint), Const.metroLineWidth);
        lineRectTransform.sizeDelta = sizeDelta;
        lineRectTransform.pivot = new Vector2(0, 0.5f);
        lineRectTransform.anchoredPosition = _startLocalPoint;

        float angle = Mathf.Atan2(_endLocalPoint.y - _startLocalPoint.y, _endLocalPoint.x - _startLocalPoint.x) * Mathf.Rad2Deg;
        lineRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// ���������յ�
    /// </summary>
    /// <param name="endLocalPoint"></param>
    public void UpdateLineEnd(Vector2 endLocalPoint)
    {
        if(_currentLineObj != null)
        {
            RectTransform lineRectTransform = _currentLineObj.GetComponent<RectTransform>();
            lineRectTransform.sizeDelta = Vector2.zero;

            _endLocalPoint = endLocalPoint;
            Vector2 sizeDelta = new Vector2(Vector2.Distance(_startLocalPoint, _endLocalPoint), Const.metroLineWidth);
            lineRectTransform.sizeDelta = sizeDelta;
            lineRectTransform.anchoredPosition = _startLocalPoint;

            float angle = Mathf.Atan2(endLocalPoint.y - _startLocalPoint.y, endLocalPoint.x - _startLocalPoint.x) * Mathf.Rad2Deg;
            lineRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /// <summary>
    /// ɾ����һ�δ�������������
    /// </summary>
    public void DeactivateCurrentLineObj()
    {
        Instantiater.DeactivateObject(_currentLineObj);
        if(CurrentMetroLine.CityNodes.Count <= 1)
        {
            metroLines.Remove(CurrentMetroLine);
        }
        _currentLineObj = null;
    }

    [Button("��ʾ��·��Ϣ")]
    public void ShowLineInfo()
    {
        foreach(var metroLine in metroLines)
        {
            foreach(var cityNode in metroLine.CityNodes)
            {
                Debug.Log($"{metroLine.name}: {cityNode.name}");
            }
        }
    }

    private void Initialize()
    {
        metroLines = new List<MetroLine>();
        metroLineColors = new List<Color>()
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            new Color(1.0f, 0.5f, 0.0f), // Orange
            new Color(0.5f, 0.0f, 0.5f), // Purple
            new Color(0.0f, 0.5f, 0.5f), // Teal
            new Color(0.5f, 0.5f, 0.0f), // Olive
            new Color(1.0f, 0.75f, 0.8f), // Pink
            new Color(0.5f, 0.5f, 0.5f), // Gray
            new Color(0.75f, 0.25f, 0.5f), // Raspberry
            new Color(0.25f, 0.75f, 0.25f), // Light Green
            new Color(0.75f, 0.75f, 0.0f), // Mustard
            new Color(0.0f, 0.75f, 0.75f), // Aqua
            new Color(0.75f, 0.0f, 0.75f), // Fuchsia
            new Color(0.75f, 0.25f, 0.0f), // Burnt Orange
            new Color(0.25f, 0.25f, 0.75f), // Indigo
            new Color(0.0f, 0.5f, 1.0f) // Sky Blue
        };

        _graphicRaycaster = GameObject.Find("CanvasRoot").GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if(GetUIMouseObj) Debug.Log($"{GetUIMouseObj.name}");
    }
}