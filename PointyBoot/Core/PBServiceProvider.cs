using System;
using System.Collections.Generic;
using System.Text;

namespace PointyBoot.Core
{
    public class PBServiceProvider : IServiceProvider
    {
        /// <summary>
        /// Context can be considered as a session which scopes the instances to a session.
        /// </summary>
        private readonly PBContext globalContext;
        
        public PBServiceProvider()
        {
            globalContext = new PBContext();            
        }

        public T Get<T>()
        {
            return globalContext.Get<T>();
        }

        public T Get<T>(IContext context)
        {
            return ((IServices)context).Get<T>();
        }

        public IServiceProvider AddSingleton<T>()
        {
            globalContext.AddSingleton<T>();
            return this;
        }

        public IServiceProvider AddSingleton<T>(IContext context)
        {
            ((IServices)context).AddSingleton<T>();
            return this;
        }

        public IServiceProvider AddSingleton<T>(object instance)
        {
            globalContext.AddSingleton<T>(instance);
            return this;
        }

        public IServiceProvider AddSingleton<T>(IContext context, object instance)
        {
            ((IServices)context).AddSingleton<T>(instance);
            return this;
        }

        public IServiceProvider AddSingleton<T>(Func<T> instantiatorFunction)
        {
            globalContext.AddSingleton(instantiatorFunction);
            return this;
        }

        public IServiceProvider AddSingleton<T>(IContext context, Func<T> instantiatorFunction)
        {
            ((IServices)context).AddSingleton(instantiatorFunction);
            return this;
        }

        public IServiceProvider RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            globalContext.RegisterFactory(factory);
            return this;
        }

        public IServiceProvider RegisterFactory<T>(IContext context, Func<T> factory)
            where T : class
        {
            ((IServices)context).RegisterFactory(factory);
            return this;
        }

        public IServiceProvider RegisterComponentFactory<T>(T instance = null)
            where T : class
        {
            if(instance == null)
                instance = globalContext.Get<T>();

            globalContext.RegisterComponentFactory(instance);
            return this;
        }

        public IServiceProvider RegisterComponentFactory<T>(IContext context, T instance = null)
             where T : class
        {
            if (instance == null)
                instance = globalContext.Get<T>();

            ((IServices)context).RegisterComponentFactory(instance);
            return this;
        }
    }
}
