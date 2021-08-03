using PointyBoot.Core;
using PointyBoot.Core.Context;
using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace PointyBoot.Base
{
    public class PointyBootDIService : IDIProviderService
    {
        private readonly IDIContext currentContext;

        public PointyBootDIService(IDIContext context = null)
        {
            if (context == null)
                currentContext = PBServicesFactory.GetGlobalContext();
        }

        public IDIProviderService AddSingleton<T>()
        {
            currentContext.AddSingleton<T>();
            return this;
        }

        public IDIProviderService AddSingleton<T>(object instanceObj)
        {
            currentContext.AddSingleton<T>(instanceObj);
            return this;
        }

        public IDIProviderService AddSingleton<T>(Func<T> instantiatorFunction)
        {
            currentContext.AddSingleton(instantiatorFunction);
            return this;
        }

        public T Get<T>()
        {
            return currentContext.Get<T>();
        }

        public IDIProviderService RegisterComponentFactory<T>(T instance) where T : class
        {
            currentContext.RegisterComponentFactory(instance);
            return this;
        }

        public IDIProviderService RegisterFactory<T>(Func<T> factory) where T : class
        {
            currentContext.RegisterFactory(factory);
            return this;
        }

        public IDIProviderService StartNewSession()
        {
            return new PointyBootDIService(PBContextFactory.GetNewContext());
        }
    }
}
