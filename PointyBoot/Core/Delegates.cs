namespace PointyBoot.Core
{
    public delegate object GenericActivator(params object[] args);
    public delegate T StronglyTypedActivator<T>(params object[] args);
}
