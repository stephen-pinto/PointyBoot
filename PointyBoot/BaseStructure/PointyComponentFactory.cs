namespace PointyBoot.BaseStructure
{
    public abstract class PointyComponentFactory : IPointyComponentFactory
    {
        public PointyComponentFactory()
        {
            Initialize();
        }

        ~PointyComponentFactory()
        {
            OnClose();
        }

        public abstract void Initialize();

        public abstract void OnClose();

        public abstract void OnLoad();
    }
}
