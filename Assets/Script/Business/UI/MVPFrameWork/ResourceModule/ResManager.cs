using System;

namespace MVPFrameWork.Singleton
{
    public class ResManager : Singleton<ResManager>, IResourceLoader
    {
        private IResourceLoader _loader;

        public void SetupLoader(IResourceLoader loader)
        {
            _loader = loader;
        }

        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            SetupLoader();
            IResourceLoader loader = _loader;
            return (loader != null) ? loader.LoadAsset<T>(assetPath) : null;
        }

        public void LoadAssetAsync<T>(string assetPath, Action<T> callback, Action<float> onProgress = null) where T : UnityEngine.Object
        {
            SetupLoader();
            _loader?.LoadAssetAsync(assetPath, callback, onProgress);
        }

        /*
        public async UniTask<T> LoadAssetTaskAsync<T>(string assetPath, Action<float> onProgress = null) where T : UnityEngine.Object
        {
            UniTaskCompletionSource<T> src = new UniTaskCompletionSource<T>();
            LoadAssetAsync(assetPath, delegate (T res)
            {
                src.TrySetResult(res);
            }, onProgress);
            return await src.Task;
        }
        */

        public void Unload(string assetPath)
        {
            SetupLoader();
            _loader?.Unload(assetPath);
        }

        public void UnloadAll()
        {
            SetupLoader();
            _loader?.UnloadAll();
        }

        private void SetupLoader()
        {
            if(_loader == null)
            {
                // _loader = new AddressablesLoader();
            }
        }
    }
}