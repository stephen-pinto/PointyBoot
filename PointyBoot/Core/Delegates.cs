namespace PointyBoot.Core
{
    public delegate object ObjectActivator(params object[] args);
    public delegate T SpecificObjectActivator<T>(params object[] args);
}
