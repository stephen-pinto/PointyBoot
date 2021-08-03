using System;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIServiceProvider
    {
        T Get<T>();

        T Get<T>(IContext context);

        IDIServiceProvider AddSingleton<T>();

        IDIServiceProvider AddSingleton<T>(IContext context);

        IDIServiceProvider AddSingleton<T>(object instanceObj);

        IDIServiceProvider AddSingleton<T>(IContext context, object instanceObj);

        IDIServiceProvider AddSingleton<T>(Func<T> instantiatorFunction);

        IDIServiceProvider AddSingleton<T>(IContext context, Func<T> instantiatorFunction);

        IDIServiceProvider RegisterFactory<T>(Func<T> factory) where T : class;

        IDIServiceProvider RegisterFactory<T>(IContext context, Func<T> factory) where T : class;

        IDIServiceProvider RegisterComponentFactory<T>(T instance) where T : class;

        IDIServiceProvider RegisterComponentFactory<T>(IContext context, T instance) where T : class;
    }
}
