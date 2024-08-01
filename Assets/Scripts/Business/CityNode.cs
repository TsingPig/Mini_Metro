using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public MetroLine MetroLine => _metroLine;

    private MetroLine _metroLine;
}
