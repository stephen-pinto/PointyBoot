using PointyBoot.Core;
using PointyBoot.Core.Context;
using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace PointyBoot.Base
{
    public class PointyBootDIService : IDIProviderService
    {
        private readonly IDIService serviceProvider;

        public PointyBootDIService()
        {
            serviceProvider = PBServicesFactory.GetDefaultServiceProvider();
        }

        public IDIProviderService AddMap<IntfType, ActType>() where ActType : IntfType
        {
            serviceProvider.AddMapping<IntfType, ActType>();
            return this;
        }

        public IDIProviderService AddSingleton<T>()
        {
            serviceProvider.AddSingleton<T>();
            return this;
        }

        public IDIProviderService AddSingleton<T>(object instanceObj)
        {
            serviceProvider.AddSingleton<T>(instanceObj);
            return this;
        }

        public IDIProviderService AddSingleton<T>(Func<T> instantiatorFunction)
        {
            serviceProvider.AddSingleton(instantiatorFunction);
            return this;
        }

        public T Get<T>()
        {
            return serviceProvider.Get<T>();
        }

        public IDIProviderService RegisterComponentFactory<T>(T instance) where T : class
        {
            serviceProvider.RegisterComponentFactory(instance);
            return this;
        }

        public IDIProviderService RegisterFactory<T>(Func<T> factory) where T : class
        {
            serviceProvider.RegisterFactory(factory);
            return this;
        }

        public IDIProviderService StartNewSession()
        {
            return new PointyBootDIService();
        }
    }
}
