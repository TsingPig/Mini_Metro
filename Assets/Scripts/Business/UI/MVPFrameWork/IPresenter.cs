namespace MVPFrameWork
{
    public interface IPresenter
    {
        IView View { get; set; }

        IModel Model { get; set; }

        void Install();

        void Uninstall();

        void OnCreateCompleted();

        void OnShowStart();

        void OnShowCompleted();

        void OnHideStart();

        void OnHideCompleted();
    }
}