using PointyBoot.Core.Interfaces;
using System;

namespace PointyBoot.Core
{
    public class PBServiceProvider : IDIService, IDIContextBasedService
    {
        /// <summary>
        /// Context can be considered as a session which scopes the instances to a session.
        /// </summary>
        private readonly IDIContext globalContext;

        public PBServiceProvider(IDIContext context)
        {
            globalContext = context;
        }

        public T Get<T>()
        {
            return globalContext.Get<T>();
        }

        public T Get<T>(IDIContext context)
        {
            return context.Get<T>();
        }

        public void AddSingleton<T>()
        {
            globalContext.AddSingleton<T>();            
        }

        public void AddSingleton<T>(IDIContext context)
        {
            context.AddSingleton<T>();
        }

        public void AddSingleton<T>(object instance)
        {
            globalContext.AddSingleton<T>(instance);
        }

        public void AddSingleton<T>(IDIContext context, object instance)
        {
            context.AddSingleton<T>(instance);
        }

        public void AddSingleton<T>(Func<T> instantiatorFunction)
        {
            globalContext.AddSingleton(instantiatorFunction);
        }

        public void AddSingleton<T>(IDIContext context, Func<T> instantiatorFunction)
        {
            context.AddSingleton(instantiatorFunction);
        }

        public void RegisterFactory<T>(Func<T> instantiatorFunction)
            where T : class
        {
            globalContext.RegisterFactory(instantiatorFunction);
        }

        public void RegisterFactory<T>(IDIContext context, Func<T> factory)
            where T : class
        {
            context.RegisterFactory(factory);
        }

        public void RegisterComponentFactory<T>(T instance = null)
            where T : class
        {
            if (instance == null)
                instance = globalContext.Get<T>();

            globalContext.RegisterComponentFactory(instance);
        }

        public void RegisterComponentFactory<T>(IDIContext context, T instance = null)
             where T : class
        {
            if (instance == null)
                instance = globalContext.Get<T>();

            context.RegisterComponentFactory(instance);
        }
    }
}
