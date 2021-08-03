using System;

namespace PointyBoot.Core
{
    public interface IServiceProvider
    {
        T Get<T>();

        T Get<T>(IContext context);

        IServiceProvider AddSingleton<T>();

        IServiceProvider AddSingleton<T>(IContext context);

        IServiceProvider AddSingleton<T>(object instanceObj);

        IServiceProvider AddSingleton<T>(IContext context, object instanceObj);

        IServiceProvider AddSingleton<T>(Func<T> instantiatorFunction);

        IServiceProvider AddSingleton<T>(IContext context, Func<T> instantiatorFunction);

        IServiceProvider RegisterFactory<T>(Func<T> factory) where T : class;

        IServiceProvider RegisterFactory<T>(IContext context, Func<T> factory) where T : class;

        IServiceProvider RegisterComponentFactory<T>(T instance) where T : class;

        IServiceProvider RegisterComponentFactory<T>(IContext context, T instance) where T : class;
    }
}
