using MVPFrameWork;
using UnityEngine;

public interface IMainView : IView
{
    RectTransform MetroLineRoot { get; set; }

    RectTransform CityNodeRoot { get; set; }
}