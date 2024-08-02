using System;
using TsingPigSDK;

namespace MVPFrameWork
{
    public sealed class UIManager : Singleton<UIManager>, IPopUIModule
    {
        private IPopUIModule _module;

        private void Init()
        {
            _module = new PopUIModule();
        }

        private new void Awake()
        {
            base.Awake();
            Init();
        }

        public void Enter(int viewId, IModel model)
        {
            _module.Enter(viewId, model);
        }

        public void Enter(int viewId, Action callback = null)
        {
            _module.Enter(viewId, callback);
        }

        public void Quit(int viewId, Action callback = null)
        {
            _module?.Quit(viewId, callback);
        }

        public void Preload(int viewId, bool instantiate = true)
        {
            _module?.Preload(viewId, instantiate);
        }
    }
}