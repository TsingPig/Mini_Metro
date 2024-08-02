using MVPFrameWork;
using TsingPigSDK;
using UnityEngine;

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    public async void GenerateCityNode()
    {
       GameObject cityNodeObj = await Instantiater.InstantiateAsync(Str.CITY_NODE_DATA_PATH, _view.CityNodeRoot);
        cityNodeObj.GetComponent<LineDrawer>().lineRoot = _view.MetroLineRoot;
    }
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        GenerateCityNode();
    }
}