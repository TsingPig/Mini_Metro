using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class MetroLineManager : Singleton<MetroLineManager>
{
    public RectTransform metroLineRoot;

    public List<MetroLine> metroLines;

    [HideInInspector]
    public List<Color> metroLineColors;

    private int MetroLineCount
    {
        get { return metroLines.Count; }
    }
    private GameObject _currentLineObj;

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
                return metroLineRoot.GetChild(MetroLineCount - 1).GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("MetroLineCount is Zero");
                return null;
            }
        }
    }

    public MetroLine CreateMetroLine()
    {
        GameObject metroLineObj = new GameObject($"MetroLine{metroLines.Count}");
        metroLineObj.transform.parent = metroLineRoot;
        metroLineObj.AddComponent<RectTransform>();
        MetroLine metroLine = metroLineObj.AddComponent<MetroLine>();
        metroLines.Add(metroLine);
        return metroLine;
    }

    [Button("CreateMetroLine")]
    public async Task CreateMetroLine(List<CityNode> cityNodes, Color metroLineColor)
    {
        GameObject metroLineObj = new GameObject($"MetroLine{metroLines.Count}");
        metroLineObj.transform.parent = metroLineRoot;

        metroLineObj.AddComponent<RectTransform>();
        MetroLine metroLine = metroLineObj.AddComponent<MetroLine>();

        metroLine.name = $"MetroLine{metroLines.Count}";
        metroLine.cityNodes = cityNodes;
        metroLine.metroLineColor = metroLineColor;

        metroLines.Add(metroLine);
        foreach(CityNode cityNode in cityNodes)
        {
            cityNode.MetroLine = metroLine;
        }
        for(int i = 0; i < cityNodes.Count - 1; i++)
        {
            await DrawLineBetween(cityNodes[i].GetComponent<RectTransform>(),
                            cityNodes[i + 1].GetComponent<RectTransform>(),
                            metroLine.GetComponent<RectTransform>(),
                            metroLineColor);
        }
    }


    public void UpdateLinePosition(Vector2 startLocalPoint, Vector2 endLocalPoint)
    {
        if(_currentLineObj != null)
        {

            RectTransform lineRectTransform = _currentLineObj.GetComponent<RectTransform>();
            lineRectTransform.sizeDelta = Vector2.zero;

            Vector2 sizeDelta = new Vector2(Vector2.Distance(startLocalPoint, endLocalPoint), Const.metroLineWidth);
            lineRectTransform.sizeDelta = sizeDelta;
            lineRectTransform.anchoredPosition = startLocalPoint;

            float angle = Mathf.Atan2(endLocalPoint.y - startLocalPoint.y, endLocalPoint.x - startLocalPoint.x) * Mathf.Rad2Deg;
            lineRectTransform.localRotation = Quaternion.Euler(0, 0, angle);

        }
    }

    public async Task DrawLineBetween(Vector2 startLocalPoint, Vector2 endLocalPoint, RectTransform lineRoot, Color color)
    {
        _currentLineObj = await Instantiater.InstantiateAsync(Str.LINE_PREFAB_DATA_PATH, lineRoot);
        _currentLineObj.GetComponent<Image>().color = color;

        RectTransform lineRectTransform = _currentLineObj.GetComponent<RectTransform>();
        lineRectTransform.sizeDelta = Vector2.zero;

        Vector2 sizeDelta = new Vector2(Vector2.Distance(startLocalPoint, endLocalPoint), Const.metroLineWidth);
        lineRectTransform.sizeDelta = sizeDelta;
        lineRectTransform.pivot = new Vector2(0, 0.5f);
        lineRectTransform.anchoredPosition = startLocalPoint;

        float angle = Mathf.Atan2(endLocalPoint.y - startLocalPoint.y, endLocalPoint.x - startLocalPoint.x) * Mathf.Rad2Deg;
        lineRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    
        
    }



    public async Task DrawLineBetween(RectTransform startRect, RectTransform endRect, RectTransform lineRoot, Color color)
    {
        Vector2 startLocalPoint;
        Vector2 endLocalPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineRoot, startRect.position, null, out startLocalPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineRoot, endRect.position, null, out endLocalPoint);
        await DrawLineBetween(startLocalPoint, endLocalPoint, lineRoot, color);
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
    }

    private void Start()
    {
        Initialize();
    }
}