using MVPFrameWork;
using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    private async Task GenerateCityNode()
    {

        GameObject cityNodeObj = await Instantiater.InstantiateAsync(Str.CITY_NODE_DATA_PATH, _view.CityNodeRoot);
        cityNodeObj.GetComponent<LineDrawer>().lineRoot = _view.MetroLineRoot;
        cityNodeObj.GetComponent<RectTransform>().localPosition += new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
        cityNodeObj.GetComponent<CityNode>().cityNodeType = (CityNodeType)Random.Range(0, 3);
        cityNodeObj.GetComponent<CityNode>().UpdateCityNodeImage();
    }

    public async void GenerateCityNodes()
    {
        for(int i = 0; i <15; i++)
        {
            await GenerateCityNode();
        }

    }
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        GenerateCityNodes();

        MetroLineManager.Instance.metroLineRoot = _view.MetroLineRoot;
    }
}