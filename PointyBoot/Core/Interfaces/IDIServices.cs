using System;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIServices
    {
        T Get<T>();

        void RegisterComponentFactory<T>(T instance);

        void AddSingleton<T>();

        void AddSingleton<T>(object instance);

        void AddSingleton<T>(Func<T> instantiatorFunction);

        void RegisterFactory<T>(Func<T> factory);
    }
}
