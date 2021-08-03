using PointyBoot.Core.Context;
using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointyBoot.Core
{
    public static class PBServicesFactory
    {
        private static Lazy<PBActivatorStore> activatorStore = new Lazy<PBActivatorStore>();
        private static Lazy<IDIContext> context = new Lazy<IDIContext>(() => PBContextFactory.GetNewContext());

        internal static IDIContext GetGlobalContext()
        {
            return context.Value;
        }

        public static IDIServiceProvider GetServiceProvider()
        {
            var obj = new PBServiceProvider(context.Value);
            return obj;
        }

        public static IActivatorStore GetActivatorStore()
        {
            return activatorStore.Value;
        }

        public static IOCProvider GetIOCProvider()
        {
            return new IOCProvider(context.Value);
        }
    }
}
