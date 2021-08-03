using PointyBoot.Core.Interfaces;
using System;

namespace PointyBoot.Core
{
    public class PBServiceProvider : IDIServiceProvider
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

        public IDIServiceProvider AddSingleton<T>()
        {
            globalContext.AddSingleton<T>();
            return this;
        }

        public IDIServiceProvider AddSingleton<T>(IDIContext context)
        {
            context.AddSingleton<T>();
            return this;
        }

        public IDIServiceProvider AddSingleton<T>(object instance)
        {
            globalContext.AddSingleton<T>(instance);
            return this;
        }

        public IDIServiceProvider AddSingleton<T>(IDIContext context, object instance)
        {
            context.AddSingleton<T>(instance);
            return this;
        }

        public IDIServiceProvider AddSingleton<T>(Func<T> instantiatorFunction)
        {
            globalContext.AddSingleton(instantiatorFunction);
            return this;
        }

        public IDIServiceProvider AddSingleton<T>(IDIContext context, Func<T> instantiatorFunction)
        {
            context.AddSingleton(instantiatorFunction);
            return this;
        }

        public IDIServiceProvider RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            globalContext.RegisterFactory(factory);
            return this;
        }

        public IDIServiceProvider RegisterFactory<T>(IDIContext context, Func<T> factory)
            where T : class
        {
            context.RegisterFactory(factory);
            return this;
        }

        public IDIServiceProvider RegisterComponentFactory<T>(T instance = null)
            where T : class
        {
            if (instance == null)
                instance = globalContext.Get<T>();

            globalContext.RegisterComponentFactory(instance);
            return this;
        }

        public IDIServiceProvider RegisterComponentFactory<T>(IDIContext context, T instance = null)
             where T : class
        {
            if (instance == null)
                instance = globalContext.Get<T>();

            context.RegisterComponentFactory(instance);
            return this;
        }
    }
}
