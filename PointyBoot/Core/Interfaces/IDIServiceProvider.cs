using System;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIServiceProvider
    {
        T Get<T>();

        T Get<T>(IDIContext context);

        IDIServiceProvider AddSingleton<T>();

        IDIServiceProvider AddSingleton<T>(IDIContext context);

        IDIServiceProvider AddSingleton<T>(object instanceObj);

        IDIServiceProvider AddSingleton<T>(IDIContext context, object instanceObj);

        IDIServiceProvider AddSingleton<T>(Func<T> instantiatorFunction);

        IDIServiceProvider AddSingleton<T>(IDIContext context, Func<T> instantiatorFunction);

        IDIServiceProvider RegisterFactory<T>(Func<T> factory) where T : class;

        IDIServiceProvider RegisterFactory<T>(IDIContext context, Func<T> factory) where T : class;

        IDIServiceProvider RegisterComponentFactory<T>(T instance) where T : class;

        IDIServiceProvider RegisterComponentFactory<T>(IDIContext context, T instance) where T : class;
    }
}
