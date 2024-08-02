using System;
using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;

namespace MVPFrameWork
{
    public abstract class ViewBase<TPresenter> : IView where TPresenter : class, IPresenter
    {
        protected RectTransform _root;

        protected CanvasGroup _rootCanvas;

        protected bool _rootCanvasIsActive;

        protected TPresenter _presenter;

        private bool _created = false;

        private string _resPath;

        public virtual bool Active
        {
            get
            {
                return _rootCanvasIsActive;
            }
            set
            {
                CanvasAlpha(!value, 0.25f, () =>
                {
                    _rootCanvasIsActive = value;
                    _rootCanvas.blocksRaycasts = value;
                    _rootCanvas.interactable = value;
                });

                //_rootCanvasIsActive = value;
                //_rootCanvas.alpha = value ? 1f : 0f;
                //_rootCanvas.blocksRaycasts = value;
                //_rootCanvas.interactable = value;
            }
        }

        /// 控制面板Alpha值动画淡入淡出
        /// </summary>
        /// <param name="fade">为真表示淡出，否则表示淡入</param>
        /// <param name="swiftTime">淡入淡出时间</param>
        private async void CanvasAlpha(bool fade, float swiftTime, Action callback = null)
        {
            float startAlpha = fade ? 1f : 0f;
            float targetAlpha = fade ? 0f : 1f;

            float elapsedTime = 0f;

            while(elapsedTime < swiftTime)
            {
                _rootCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / swiftTime);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            _rootCanvas.alpha = targetAlpha;
            callback?.Invoke();
        }

        public IPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if(_presenter != null)
                {
                    _presenter.Uninstall();
                }

                _presenter = value as TPresenter;
                if(_presenter != null)
                {
                    _presenter.View = this;
                    _presenter.Install();
                }
            }
        }

        public ViewBase()
        {
            Presenter = Container.Resolve<TPresenter>();
        }

        public void Preload(Action callback = null, bool instantiate = true)
        {
            if(_created)
            {
                return;
            }

            ParseResInfo(out var assetPath, out var _);
            _resPath = assetPath;
            UISetting.DefaultResoucesLoader?.LoadAssetAsync(assetPath, delegate (GameObject obj)
            {
                if(instantiate)
                {
                    OnGetResInfoCompleted(obj);
                    Active = false;
                }

                callback?.Invoke();
            });
        }

        public async void Create(Action callback = null)
        {
            if(_created)
            {
                callback?.Invoke();
                return;
            }

            ParseResInfo(out var assetPath, out var async);
            _resPath = assetPath;
            var handle = Res<GameObject>.LoadAsync(_resPath);
            await handle;
            GameObject obj = handle.Result;

            OnGetResInfoCompleted(obj);
            callback?.Invoke();

            //if(async)
            //{
            //    UISetting.DefaultResoucesLoader?.LoadAssetAsync(assetPath, delegate (GameObject obj)
            //    {
            //        OnGetResInfoCompleted(obj);
            //        callback?.Invoke();
            //    });
            //}
            //else
            //{
            //    GameObject obj2 = UISetting.DefaultResoucesLoader?.LoadAsset<GameObject>(assetPath);
            //    OnGetResInfoCompleted(obj2);
            //    callback?.Invoke();
            //}
        }

        public void Show(Action callback = null)
        {
            try
            {
                _presenter?.OnShowStart();
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
            }

            try
            {
                OnShow(delegate
                {
                    try
                    {
                        _presenter?.OnShowCompleted();
                    }
                    catch(Exception exception3)
                    {
                        Debug.LogException(exception3);
                    }

                    callback?.Invoke();
                });
            }
            catch(Exception exception2)
            {
                Debug.LogException(exception2);
            }
        }

        public void Hide(Action callback = null)
        {
            try
            {
                _presenter?.OnHideStart();
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
            }

            try
            {
                OnHide(delegate
                {
                    try
                    {
                        _presenter?.OnHideCompleted();
                    }
                    catch(Exception exception3)
                    {
                        Debug.LogException(exception3);
                    }

                    callback?.Invoke();
                });
            }
            catch(Exception exception2)
            {
                Debug.LogException(exception2);
            }
        }

        public void Destroy()
        {
            Debug.Log("ViewBase Destory");
            OnDestroy();
            Presenter = null;
            UnityEngine.Object.DestroyImmediate(_root.gameObject);
        }

        protected abstract void OnCreate();

        protected virtual void OnShow(Action callback)
        {
            Active = true;
            callback?.Invoke();
        }

        protected virtual void OnHide(Action callback)
        {
            Active = false;
            callback?.Invoke();
        }

        protected virtual void OnDestroy()
        {
        }

        private void ParseResInfo(out string assetPath, out bool async)
        {
            assetPath = string.Empty;
            async = false;
            Type type = GetType();
            object[] customAttributes = type.GetCustomAttributes(typeof(ResInfoAttribute), inherit: true);
            if(customAttributes != null)
            {
                object[] array = customAttributes;
                for(int i = 0; i < array.Length; i++)
                {
                    ResInfoAttribute resInfoAttribute = (ResInfoAttribute)array[i];
                    if(resInfoAttribute != null)
                    {
                        assetPath = resInfoAttribute.assetPath;
                        async = resInfoAttribute.async;
                    }
                }
            }

            if(string.IsNullOrEmpty(assetPath))
            {
                string name = type.Name;
                assetPath = "Assets/Res/UI/" + name + "/" + name + ".prefab";
            }
            Debug.Log("ViewBase" + assetPath);
        }

        private Transform ParseParentAttr()
        {
            Transform result = null;
            FindType type = FindType.None;
            string param = string.Empty;
            GenerateDefaultParentInfo(ref type, ref param);
            Type type2 = GetType();
            object[] customAttributes = type2.GetCustomAttributes(typeof(ParentInfoAttribute), inherit: true);
            if(customAttributes != null)
            {
                object[] array = customAttributes;
                for(int i = 0; i < array.Length; i++)
                {
                    ParentInfoAttribute parentInfoAttribute = (ParentInfoAttribute)array[i];
                    if(parentInfoAttribute != null)
                    {
                        type = parentInfoAttribute.type;
                        param = parentInfoAttribute.param;
                    }
                }
            }

            switch(type)
            {
                case FindType.FindWithTag:
                result = NodeContainer.FindNodeWithTag(param);
                break;

                case FindType.FindWithName:
                result = NodeContainer.FindNodeWithName(param);
                break;
            }

            return result;
        }

        private void GenerateDefaultParentInfo(ref FindType type, ref string param)
        {
            if(UISetting.DefaultParentParam != null)
            {
                type = UISetting.DefaultParentParam.findType;
                param = UISetting.DefaultParentParam.param;
            }
            else
            {
                type = FindType.FindWithName;
                param = "Canvas";
            }
        }

        private void OnGetResInfoCompleted(GameObject obj)
        {
            if(obj != null)
            {
                Transform parent = ParseParentAttr();
                _root = UnityEngine.Object.Instantiate(obj, parent).GetComponent<RectTransform>();
                if(_root != null)
                {
                    _rootCanvas = _root.GetComponent<CanvasGroup>();
                    if(_rootCanvas == null)
                    {
                        _rootCanvas = _root.gameObject.AddComponent<CanvasGroup>();
                    }

                    OnCreate();
                    _created = true;
                    _presenter?.OnCreateCompleted();
                    return;
                }

                throw new Exception("<Ming> ## Uni Exception ## Cls:" + GetType().Name + " Func:Create Info:Instantiate failed !");
            }

            throw new Exception("<Ming> ## Uni Exception ## Cls:" + GetType().Name + " Func:Create Info:Load res failed !");
        }
    }
}