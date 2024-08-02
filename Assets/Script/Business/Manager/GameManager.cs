using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    private void Init()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
        Debug.Log("Éú³É£º" + MetroLineManager.Instance);
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}