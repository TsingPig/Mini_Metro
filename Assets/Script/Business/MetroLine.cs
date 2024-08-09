using UnityEngine.UI;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;

public class MetroLine : MonoBehaviour
{
    public string metroLineName;
    public Color metroLineColor;
    public RectTransform metroTrainRoot;
    public RectTransform lineRoot;

    public List<MetroTrain> MetroTrains { get => _metroTrains; set { _metroTrains = value; } }
    public List<CityNode> CityNodes
    {
        get
        {

            return _cityNodes;
        }
        set
        {
            _cityNodes = value;

        }
    }


    private List<CityNode> _cityNodes = new List<CityNode>();
    private List<MetroTrain> _metroTrains = new List<MetroTrain>();

    private void Update()
    {

        if(CityNodes.Count == 2 && _metroTrains.Count == 0)
        {

            GameObject metroTrainObj = Instantiater.Instantiate(Str.METRO_TRAIN_DATA_PATH, metroTrainRoot);
            MetroTrain metroTrain = metroTrainObj.GetComponent<MetroTrain>();
            metroTrain.CurrentMetroLine = this;
            metroTrain.GetComponent<Image>().color = metroLineColor;
            metroTrain.transform.position = _cityNodes[0].transform.position;
            _metroTrains.Add(metroTrain);
        }
    }
}