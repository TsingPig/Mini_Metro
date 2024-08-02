namespace MVPFrameWork
{
    public abstract class PresenterBase<TView, TModel> : IPresenter where TView : class, IView where TModel : class, IModel
    {
        protected TView _view;

        protected TModel _model;

        public IView View
        {
            get => _view;
            set => _view = value as TView;
        }

        public IModel Model
        {
            get => _model;
            set => _model = value as TModel;
        }

        public virtual void Install()
        {
        }

        public virtual void Uninstall()
        {
        }

        public virtual void OnCreateCompleted()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnShowStart()
        {
        }

        public virtual void OnShowCompleted()
        {
        }

        public virtual void OnHideStart()
        {
        }

        public virtual void OnHideCompleted()
        {
        }

        public virtual void ConfigModel()
        { }
    }

    public abstract class PresenterBase<TView> : IPresenter where TView : class, IView
    {
        protected TView _view;

        public IView View
        {
            get => _view;
            set => _view = value as TView;
        }

        public IModel Model { get; set; }

        public virtual void Install()
        {
        }

        public virtual void Uninstall()
        {
        }

        public virtual void OnCreateCompleted()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnShowStart()
        {
        }

        public virtual void OnShowCompleted()
        {
        }

        public virtual void OnHideStart()
        {
        }

        public virtual void OnHideCompleted()
        {
        }

        public virtual void ConfigModel()
        { }
    }
}