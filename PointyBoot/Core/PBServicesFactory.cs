using PointyBoot.Core.Context;
using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointyBoot.Core
{
    public static class PBServicesFactory
    {
        private static Lazy<PBActivatorCache> activatorStore = new Lazy<PBActivatorCache>();
        private static Lazy<IDIContext> globalContext = new Lazy<IDIContext>(() => PBContextFactory.GetNewContext());

        internal static IDIContext GetGlobalContext()
        {
            return globalContext.Value;
        }

        public static IDIService GetDefaultServiceProvider()
        {
            var obj = new PBServiceProvider(globalContext.Value, GetIOCProvider());
            return obj;
        }

        public static IDIService GetServiceProviderForContext(IDIContext context)
        {
            var obj = new PBServiceProvider(context, GetIOCProvider());
            return obj;
        }

        public static IActivatorStore GetGlobalActivatorCache()
        {
            return activatorStore.Value;
        }

        public static IOCProvider GetIOCProvider()
        {
            return new IOCProvider();
        }
    }
}
