using System;
using System.Collections.Generic;
using TsingPigSDK;

namespace MVPFrameWork
{
    public class UIModule : IUIModule
    {
        private Dictionary<int, IView> _uiDic = new Dictionary<int, IView>();

        private IView this[int viewId]
        {
            get
            {
                IView value;
                _uiDic.TryGetValue(viewId, out value);
                return value;
            }
            set
            {
                _uiDic[viewId] = value;
            }
        }

        public void Enter(int viewId, IModel model)
        {
            IView view = this[viewId];
            if(view == null)
            {
                Log.Info("Enter£∫", viewId.ToString());

                view = Container.Resolve<IView>(viewId);

                if(view == null)
                {
                    Log.Error($"{viewId}Ω‚Œˆ ß∞‹");
                }

                view.Presenter.Model = model;

                view?.Create(delegate
                {
                    Log.Info(viewId.ToString() + "UIModule Enter");
                    this[viewId] = view;
                });
            }
            else
            {
                view.Presenter.Model = model;
                view.Show();
            }
        }

        public void Enter(int viewId, Action callback = null)
        {
            IView view = this[viewId];
            if(view == null)
            {
                Log.Info("Enter£∫", viewId.ToString());

                view = Container.Resolve<IView>(viewId);

                if(view == null)
                {
                    Log.Error($"{viewId}Ω‚Œˆ ß∞‹");
                }

                view?.Create(delegate
                {
                    Log.Info(viewId.ToString() + "UIModule Enter");

                    this[viewId] = view;
                    view?.Show(callback);
                });
            }
            else
            {
                view.Show(callback);
            }
        }

        public void Quit(int viewId, Action callback = null, bool destroy = false)
        {
            IView view = this[viewId];
            if(view == null)
            {
                return;
            }

            view.Hide(delegate
            {
                Log.Info(viewId.ToString() + "UIModule Quit");

                if(destroy)
                {
                    view.Destroy();
                    _uiDic.Remove(viewId);
                }

                callback?.Invoke();
            });
        }

        public void Preload(int viewId, bool instantiate = true)
        {
            Log.Info("Preload");
            IView view = this[viewId];
            if(view != null)
            {
                return;
            }

            view = Container.Resolve<IView>(viewId);
            view?.Preload(delegate
            {
                IView view2 = this[viewId];
                if(instantiate && view2 == null)
                {
                    this[viewId] = view;
                    view.Active = false;
                }
            }, instantiate);
        }
    }
}