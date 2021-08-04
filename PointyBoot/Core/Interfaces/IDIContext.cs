using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIContext : IDIService, IDisposable
    {
        IReadOnlyDictionary<Type, Type> TypeMapping { get; }

        IReadOnlyDictionary<Type, object> SingletonStore { get; }

        IReadOnlyDictionary<Type, Func<object>> FactoryFunctionStore { get; }
    }
}
