using System;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;

namespace MVPFrameWork
{
    public sealed class PopUIModule : IPopUIModule
    {
        private struct ViewState
        {
            public bool active;
        }

        private IUIModule _uiModule;

        private Dictionary<int, ViewState> _viewDic = new Dictionary<int, ViewState>();

        private List<int> _tempQuitList = new List<int>();

        public PopUIModule()
        {
            _uiModule = new UIModule();
        }

        public void Enter(int viewId, IModel model)
        {
            _viewDic.TryGetValue(viewId, out var value);
            Debug.Log(" Enter(int viewId, IModel model)");

            if(!value.active)
            {
                Debug.Log("!value.active");
                value.active = true;
                _viewDic[viewId] = value;
                _uiModule?.Enter(viewId, model);
            }
        }

        public void Enter(int viewId, Action callback = null)
        {
            _viewDic.TryGetValue(viewId, out var value);
            if(!value.active)
            {
                value.active = true;
                _viewDic[viewId] = value;
                _uiModule?.Enter(viewId, delegate
                {
                    callback?.Invoke();
                });
            }
            else
            {
                callback?.Invoke();
            }
        }

        public void Quit(int viewId, Action callback = null)
        {
            if(_viewDic.TryGetValue(viewId, out var value))
            {
                if(value.active)
                {
                    value.active = false;
                    _viewDic[viewId] = value;
                    _uiModule?.Quit(viewId, delegate
                    {
                        Log.Info($"{viewId}");
                        callback?.Invoke();
                    });
                }
                else
                {
                    callback?.Invoke();
                }
            }
            else
            {
                callback?.Invoke();
            }
        }

        public void Preload(int viewId, bool instantiate = true)
        {
            _uiModule?.Preload(viewId, instantiate);
        }
    }
}