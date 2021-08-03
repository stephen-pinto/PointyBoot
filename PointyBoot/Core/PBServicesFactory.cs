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

        public static IDIServiceProvider GetServiceProvider()
        {
            var context = PBContextFactory.GetNewContext();
            var obj = new PBServiceProvider(context);
            return obj;
        }

        public static IActivatorStore GetActivatorStore()
        {
            return activatorStore.Value;
        }
    }
}
