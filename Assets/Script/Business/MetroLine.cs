using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class MetroLine : MonoBehaviour
{
    public string metroLineName;
    public Color metroLineColor;
    public RectTransform metroTrainRoot;
    public RectTransform metroLineRoot;

    public List<MetroTrain> MetroTrains { get => _metroTrains; set => _metroTrains = value; }

    public List<CityNode> CityNodes { get => _cityNodes; set => _cityNodes = value; }

    private List<CityNode> _cityNodes = new List<CityNode>();
    private List<MetroTrain> _metroTrains = new List<MetroTrain>();

    private void Update()
    {
        if(CityNodes.Count == 2 && _metroTrains.Count == 0)
        {
            MetroTrain metroTrain = Instantiater.Instantiate<MetroTrain>(Str.METRO_TRAIN_DATA_PATH, metroTrainRoot);
            metroTrain.CurrentMetroLine = this;
            metroTrain.GetComponent<Image>().color = metroLineColor;
            metroTrain.transform.position = _cityNodes[0].transform.position;
            _metroTrains.Add(metroTrain);
        }
    }
}