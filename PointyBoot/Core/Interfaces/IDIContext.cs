using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Interfaces
{
    public interface IDIContext : IDisposable
    {
        /// <summary>
        /// Under each context the mapping can be maintained differently.
        /// </summary>
        IReadOnlyDictionary<Type, Type> TypeMapping { get; }

        /// <summary>
        /// Under each context the singleton objects can be different. 
        /// This could help when building one's own transient functionality like ASP.NET Core
        /// </summary>
        IReadOnlyDictionary<Type, object> SingletonStore { get; }

        /// <summary>
        /// Under each context the factories can be maintained differently.
        /// </summary>
        IReadOnlyDictionary<Type, Func<object>> FactoryFunctionStore { get; }

        void LoadComponentFactory<T>(T instance);

        void AddFactoryFunction(Type type, Func<object> func);

        void AddTypeMapping(Type intfType, Type actType);

        void AddSingleton(Type type, object instance);
    }
}
