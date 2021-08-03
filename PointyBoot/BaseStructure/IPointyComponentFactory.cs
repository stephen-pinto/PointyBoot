namespace PointyBoot.BaseStructure
{
    interface IPointyComponentFactory
    {
        void Initialize();

        void OnLoad();

        void OnClose();
    }
}
