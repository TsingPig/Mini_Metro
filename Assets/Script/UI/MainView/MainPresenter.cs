using MVPFrameWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;
using static TsingPigSDK.Math;

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    public async void GenerateCityNodes()
    {
        Vector2 sampleRegionSize = new Vector2(Const.screenDefaultWidth * 0.95f, Const.screenDefaultHeight * 0.9f);
        float radius = 300f;
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(radius, sampleRegionSize);

        int nodeCount = Mathf.Min(30, points.Count);

        for(int i = 0; i < nodeCount; i++)
        {
            Vector2 point = points[i] + new Vector2(-Const.screenDefaultWidth * 0.95f / 2, -Const.screenDefaultHeight * 0.9f / 2);
            GameObject cityNodeObj = await Instantiater.InstantiateAsync(Str.CITY_NODE_DATA_PATH, _view.CityNodeRoot);
            cityNodeObj.name = $"CityNode{i}";
            cityNodeObj.GetComponent<RectTransform>().localPosition = new Vector3(point.x, point.y, 0);
            cityNodeObj.GetComponent<CityNode>().cityNodeType = (CityNodeType)Random.Range(0, 3);
            cityNodeObj.GetComponent<CityNode>().UpdateCityNodeImage();
            cityNodeObj.GetComponent<CityNode>().name = cityNodeObj.name;
            cityNodeObj.GetComponent<CityNode>().txtName.text = i.ToString();
        }
    }



    //private async Task GenerateCityNode()
    //{
    //    GameObject cityNodeObj = await Instantiater.InstantiateAsync(Str.CITY_NODE_DATA_PATH, _view.CityNodeRoot);
    //    cityNodeObj.GetComponent<RectTransform>().localPosition += new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
    //    cityNodeObj.GetComponent<CityNode>().cityNodeType = (CityNodeType)Random.Range(0, 3);
    //    cityNodeObj.GetComponent<CityNode>().UpdateCityNodeImage();
    //}



    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        GenerateCityNodes();

        MetroLineManager.Instance.metroLineRoot = _view.MetroLineRoot;
    }
}