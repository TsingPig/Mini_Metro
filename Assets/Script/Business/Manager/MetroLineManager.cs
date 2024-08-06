using Sirenix.OdinInspector;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class MetroLineManager : Singleton<MetroLineManager>
{
    public RectTransform metroLineRoot;
    public GameObject linePrefab;

    public List<MetroLine> metroLines;

    [HideInInspector]
    public List<Color> metroLineColors;

    private int MetroLineCount
    {
        get { return metroLines.Count; }
    }

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
    public void CreateMetroLine(List<CityNode> cityNodes, Color metroLineColor)
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
            DrawLineBetween(cityNodes[i].GetComponent<RectTransform>(),
                            cityNodes[i + 1].GetComponent<RectTransform>(),
                            metroLine.GetComponent<RectTransform>(),
                            metroLineColor, Const.metroLineWidth);
        }
    }

    public void DrawLineBetween(RectTransform startRect, RectTransform endRect, RectTransform lineRoot, Color color, float width)
    {
        GameObject newLine = Instantiate(linePrefab, lineRoot);
        Image lineImage = newLine.GetComponent<Image>();
        lineImage.color = color;

        RectTransform lineRectTransform = newLine.GetComponent<RectTransform>();
        lineRectTransform.sizeDelta = Vector2.zero;

        Vector2 startLocalPoint;
        Vector2 endLocalPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineRoot, startRect.position, null, out startLocalPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineRoot, endRect.position, null, out endLocalPoint);

        Vector2 sizeDelta = new Vector2(Vector2.Distance(startLocalPoint, endLocalPoint), width);
        lineRectTransform.sizeDelta = sizeDelta;
        lineRectTransform.pivot = new Vector2(0, 0.5f);
        lineRectTransform.anchoredPosition = startLocalPoint;

        float angle = Mathf.Atan2(endLocalPoint.y - startLocalPoint.y, endLocalPoint.x - startLocalPoint.x) * Mathf.Rad2Deg;
        lineRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private async void Initialize()
    {
        linePrefab = await Instantiater.InstantiateAsync(Str.LINE_DATA_PATH, transform);
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