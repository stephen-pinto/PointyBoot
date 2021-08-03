using System;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIService
    {
        T Get<T>();

        void AddSingleton<T>();

        void AddSingleton<T>(object instanceObj);

        void AddSingleton<T>(Func<T> instantiatorFunction);

        void RegisterFactory<T>(Func<T> factory) where T : class;

        void RegisterComponentFactory<T>(T instance) where T : class;
    }
}
