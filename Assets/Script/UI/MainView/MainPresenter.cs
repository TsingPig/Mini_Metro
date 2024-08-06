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
        Vector2 sampleRegionSize = new Vector2(Const.screenDefaultWidth * 9 / 10, Const.screenDefaultHeight * 9 / 10);
        float radius = 300f;
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(radius, sampleRegionSize);

        int nodeCount = Mathf.Min(15, points.Count);

        for(int i = 0; i < nodeCount; i++)
        {
            Vector2 point = points[i] + new Vector2(-Const.screenDefaultWidth / 2, -Const.screenDefaultHeight / 2);
            GameObject cityNodeObj = await Instantiater.InstantiateAsync(Str.CITY_NODE_DATA_PATH, _view.CityNodeRoot);
            cityNodeObj.GetComponent<RectTransform>().localPosition = new Vector3(point.x, point.y, 0);
            cityNodeObj.GetComponent<CityNode>().cityNodeType = (CityNodeType)Random.Range(0, 3);
            cityNodeObj.GetComponent<CityNode>().UpdateCityNodeImage();
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