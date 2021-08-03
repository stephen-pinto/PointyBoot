using System;
using System.Collections.Generic;
using System.Text;

namespace PointyBoot.Core
{
    public interface IServices
    {
        T Get<T>();

        void RegisterComponentFactory<T>(T instance);

        void AddSingleton<T>();

        void AddSingleton<T>(object instance);

        void AddSingleton<T>(Func<T> instantiatorFunction);
    }
}
