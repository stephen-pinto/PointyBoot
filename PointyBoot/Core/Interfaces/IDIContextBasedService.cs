using System;
using System.Collections.Generic;
using System.Text;

namespace PointyBoot.Core.Interfaces
{
    interface IDIContextBasedService
    {
        T Get<T>(IDIContext context);

        void AddSingleton<T>(IDIContext context);

        void AddSingleton<T>(IDIContext context, object instanceObj);

        void AddSingleton<T>(IDIContext context, Func<T> instantiatorFunction);

        void RegisterFactory<T>(IDIContext context, Func<T> factory) where T : class;

        void RegisterComponentFactory<T>(IDIContext context, T instance) where T : class;
    }
}
