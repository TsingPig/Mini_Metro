using System;

namespace MVPFrameWork
{
    public interface IUIModule
    {
        void Enter(int viewId, Action callback = null);

        void Enter(int viewId, IModel model);

        void Quit(int viewId, Action callback = null, bool destroy = false);

        void Preload(int viewId, bool instantiate = true);
    }
}