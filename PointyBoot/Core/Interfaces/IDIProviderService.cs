using System;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIProviderService
    {
        T Get<T>();

        IDIProviderService AddSingleton<T>();

        IDIProviderService AddSingleton<T>(object instanceObj);

        IDIProviderService AddSingleton<T>(Func<T> instantiatorFunction);

        IDIProviderService RegisterFactory<T>(Func<T> factory) where T : class;

        IDIProviderService RegisterComponentFactory<T>(T instance) where T : class;

        IDIProviderService StartNewSession();
    }
}
