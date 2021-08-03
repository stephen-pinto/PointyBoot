using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIContext : IDIServices
    {
        Dictionary<Type, object> SingletonStore { get; }

        Dictionary<Type, Func<object>> FactoryFunctionStore { get; }
    }
}
