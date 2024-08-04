using MVPFrameWork;
using UnityEngine;

[ParentInfo(FindType.FindWithName, Str.CANVAS)]
public class MainView : ViewBase<IMainPresenter>, IMainView
{
    private RectTransform _metroLineRoot;
    private RectTransform _cityNodeRoot;

    public RectTransform MetroLineRoot { get => _metroLineRoot; set => _metroLineRoot = value; }

    public RectTransform CityNodeRoot { get => _cityNodeRoot; set => _cityNodeRoot = value; }

    protected override void OnCreate()
    {
        _metroLineRoot = _root.Find<RectTransform>("MetroLineRoot");
        _cityNodeRoot = _root.Find<RectTransform>("CityNodeRoot");
    }
}