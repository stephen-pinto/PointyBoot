using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIContext : IDisposable
    {
        IReadOnlyDictionary<Type, Type> TypeMapping { get; }

        IReadOnlyDictionary<Type, object> SingletonStore { get; }

        IReadOnlyDictionary<Type, Func<object>> FactoryFunctionStore { get; }

        void LoadComponentFactory<T>(T instance);

        void AddFactoryFunction(Type type, Func<object> func);

        void AddTypeMapping(Type intfType, Type actType);

        void AddSingleton(Type type, object instance);
    }
}
