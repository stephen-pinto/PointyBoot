using PointyBoot.Core.Interfaces;
using System;

namespace PointyBoot.Core
{
    public class PBServiceProvider : IDIService
    {
        private readonly IOCProvider instanceProvider;

        /// <summary>
        /// Context can be considered as a session which scopes the instances to a session.
        /// </summary>
        private readonly IDIContext currentContext;

        public PBServiceProvider(IDIContext context, IOCProvider instanceProvider)
        {
            currentContext = context;
            this.instanceProvider = instanceProvider;
        }

        public T Get<T>()
        {
            return instanceProvider.New<T>(currentContext);
        }

        public void RegisterComponentFactory<T>(T obj)
            where T : class
        {
            currentContext.LoadComponentFactory(obj);
        }

        public void RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            currentContext.AddFactoryFunction(typeof(T), factory);
        }

        public void AddMapping<IntfType, ActType>() where ActType : IntfType
        {
            if (!currentContext.TypeMapping.ContainsKey(typeof(IntfType)))
                currentContext.AddTypeMapping(typeof(IntfType), typeof(ActType));
            else
                throw new ArgumentException($"Mapping for type {typeof(IntfType).Name} is already defined");
        }

        public void AddSingleton<T>()
        {
            currentContext.AddSingleton(typeof(T), Get<T>());
        }

        public void AddSingleton<T>(object instance)
        {
            currentContext.AddSingleton(typeof(T), instance);
        }

        public void AddSingleton<T>(Func<T> instantiatorFunction)
        {
            if (instantiatorFunction is null)
                throw new ArgumentNullException(nameof(instantiatorFunction));

            currentContext.AddSingleton(typeof(T), instantiatorFunction);
        }
    }
}
