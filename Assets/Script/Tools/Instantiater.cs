using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace TsingPigSDK
{
    public static class Instantiater
    {
        private static Dictionary<string, List<GameObject>> _objectPools = new Dictionary<string, List<GameObject>>();

        public static void Release()
        {
            foreach(var addressablePath in _objectPools.Keys)
            {
                Release(addressablePath);
            }
            _objectPools.Clear();
        }

        public static void Release(string addressablePath)
        {
            if(_objectPools.ContainsKey(addressablePath))
            {
                foreach(var obj in _objectPools[addressablePath])
                {
                    Addressables.ReleaseInstance(obj);
                }
                _objectPools[addressablePath].Clear();
            }
            else
            {
                Debug.LogWarning($"{addressablePath}为空，不需要析构");
            }
        }

        public static void Release(string addressablePath, GameObject targetGameObject)
        {
            if(_objectPools.ContainsKey(addressablePath))
            {
                foreach(var gameObj in _objectPools[addressablePath])
                {
                    if(targetGameObject == gameObj)
                    {
                        _objectPools.Remove(addressablePath);
                        Addressables.ReleaseInstance(targetGameObject);
                    }
                }
            }
            else
            {
                Debug.LogError($"无法析构：{addressablePath} {targetGameObject.name}");
            }
        }

        public static async Task<GameObject> InstantiateAsync(string addressablePath, Transform parent, string objName = null)
        {
            List<GameObject> objectPool;

            if(_objectPools.TryGetValue(addressablePath, out objectPool) && objectPool.Count > 0)
            {
                foreach(var obj in objectPool)
                {
                    if(!obj.activeSelf)
                    {
                        obj.SetActive(true);
                        return obj;
                    }
                }
            }
            AsyncOperationHandle<IList<IResourceLocation>> handlers = Addressables.LoadResourceLocationsAsync(addressablePath);
            await handlers.Task;

            IList<IResourceLocation> results = handlers.Result;

            if(results.Count > 0)
            {
                AsyncOperationHandle<GameObject> handler = Addressables.InstantiateAsync(results[0], parent);
                await handler.Task;
                GameObject instantiatedObject = handler.Result;
                if(instantiatedObject != null)
                {
                    if(!_objectPools.ContainsKey(addressablePath))
                    {
                        _objectPools[addressablePath] = new List<GameObject>();
                    }
                    _objectPools[addressablePath].Add(instantiatedObject);
                    if(objName != null) instantiatedObject.name = objName;
                    return instantiatedObject;
                }
                else
                {
                    Debug.LogError($"{addressablePath}GameObject找不到文件位置");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"{addressablePath}");
                return null;
            }
        }

        public static async Task<T> InstantiateAsync<T>(string addressablePath, Transform parent, string objName = null)
        {
            GameObject obj = await InstantiateAsync(addressablePath, parent, objName);
            T comp = obj.GetComponent<T>();
            if(comp != null)
            {
                return comp;
            }
            else {
                Debug.LogError($"{addressablePath}GameObject缺少{typeof(T)}类型组件");
                return default(T);
            }
        }

        public static GameObject Instantiate(string addressablePath, Transform parent, string objName = null)
        {
            List<GameObject> objectPool;

            if(_objectPools.TryGetValue(addressablePath, out objectPool) && objectPool.Count > 0)
            {
                foreach(var obj in objectPool)
                {
                    if(!obj.activeSelf)
                    {
                        obj.SetActive(true);
                        obj.transform.SetParent(parent);
                        return obj;
                    }
                }
            }

            AsyncOperationHandle<GameObject> handler = Addressables.InstantiateAsync(addressablePath, parent);
            handler.WaitForCompletion();
            GameObject instantiatedObject = handler.Result;
            if(instantiatedObject != null)
            {
                if(!_objectPools.ContainsKey(addressablePath))
                {
                    _objectPools[addressablePath] = new List<GameObject>();
                }
                _objectPools[addressablePath].Add(instantiatedObject);
                if(objName != null) instantiatedObject.name = objName;
                return instantiatedObject;
            }
            else
            {
                Debug.LogError($"{addressablePath} GameObject找不到文件位置");
                return null;
            }
        }

        public static T Instantiate<T>(string addressablePath, Transform parent, string objName = null)
        {
            GameObject obj = Instantiate(addressablePath, parent, objName);
            T comp = obj.GetComponent<T>();
            if(comp != null)
            {
                return comp;
            }
            else
            {
                Debug.LogError($"{addressablePath}GameObject缺少{typeof(T)}类型组件");
                return default(T);
            }
        }

        public static void DeactivateObjectPool(string addressablePath)
        {
            if(_objectPools.ContainsKey(addressablePath))
            {
                foreach(var obj in _objectPools[addressablePath])
                {
                    DeactivateObject(obj);
                }
            }
            else
            {
                Debug.LogWarning($"{addressablePath}不在对象池中");
            }
        }

        public static int DeactivateObjectById(string addressablePath, int id)
        {
            int index = 0;
            for(int i = 0; i < id; i++)
            {
                if(_objectPools[addressablePath][i].activeSelf)
                {
                    index++;
                }
            }
            if(_objectPools.ContainsKey(addressablePath))
            {
                if(id < _objectPools[addressablePath].Count)
                {
                    _objectPools[addressablePath][id].SetActive(false);
                }
                else
                {
                    Debug.LogWarning($"{id} out of bound");
                }
            }
            else
            {
                Debug.LogWarning($"{addressablePath} not in object pool");
            }
            return index;
        }

        public static void DeactivateObject(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}