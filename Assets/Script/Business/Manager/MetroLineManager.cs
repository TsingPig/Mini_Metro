using Sirenix.OdinInspector;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class MetroLineManager : Singleton<MetroLineManager>
{
    public List<MetroLine> metroLines;

    [HideInInspector] public RectTransform metroLineRoot;
    public GameObject linePrefab;

    public float metroLineWidth = Const.metroLineWidth;

    private async void Initialize()
    {
        linePrefab = await Instantiater.InstantiateAsync(Str.LINE_DATA_PATH, transform);
    }

    private void Start()
    {
        metroLines = new List<MetroLine>();
        Initialize();
    }

    public MetroLine CreateMetroLine()
    {
        GameObject metroLineObj = new GameObject($"MetroLine{metroLines.Count}");
        metroLineObj.transform.parent = metroLineRoot;
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
                            metroLineColor, metroLineWidth);
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

    
}