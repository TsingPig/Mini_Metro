using MVPFrameWork;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    private void Initialize()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
        Debug.Log("Éú³É£º" + MetroLineManager.Instance);

        ApplicationEntry();
    }

    private new void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void ApplicationEntry()
    {
        UIManager.Instance.Enter(ViewId.MainView);
    }
}