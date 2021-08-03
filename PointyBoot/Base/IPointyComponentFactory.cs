namespace PointyBoot.Base
{
    interface IPointyComponentFactory
    {
        void Initialize();

        void OnLoad();

        void OnClose();
    }
}
