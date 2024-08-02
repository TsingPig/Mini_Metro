using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TsingPigSDK
{
    public static class Res<T>
    {
        public static Dictionary<string, T> _dicResCache = new Dictionary<string, T> { };

        public static AsyncOperationHandle<T> handle;

        public static void Release()
        {
            Addressables.Release(handle);
        }

        public static T Load(string path)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path + "1");
            Log.CallInfo($"{handle.Result}异步加载完成");
            T result = handle.WaitForCompletion();
            //Addressables.Release(handle);
            return result;
        }

        public static async Task<T> LoadAsync(string path)
        {
            if(_dicResCache.ContainsKey(path))
            {
                return _dicResCache[path];
            }

            handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;
            Log.CallInfo($"{handle.Result}异步加载完成");
            T result = handle.Result;

            _dicResCache.Add(path, result);
            return result;
        }
    }
}