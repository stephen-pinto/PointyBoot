using PointyBoot.Core.Interfaces;
using System;

namespace PointyBoot.Core.Context
{
    public static class PBContextFactory
    {
        private static Lazy<PBActivatorStore> defaultSharedInfo = new Lazy<PBActivatorStore>();

        public static PBActivatorStore GetSharedInfo()
        {
            return defaultSharedInfo.Value;
        }

        public static IDIContext GetNewContext(PBContextInfo contextInfo = null)
        {
            if (contextInfo == null)
                contextInfo = new PBContextInfo();

            return new PBContext(contextInfo);
        }
    }
}
