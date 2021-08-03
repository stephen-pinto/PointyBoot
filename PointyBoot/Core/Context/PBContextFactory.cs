using PointyBoot.Core.Interfaces;
using System;

namespace PointyBoot.Core.Context
{
    public static class PBContextFactory
    {
        public static IDIContext GetNewContext(PBContextInfo contextInfo = null)
        {
            if (contextInfo == null)
                contextInfo = new PBContextInfo();

            return new PBContext(contextInfo);
        }
    }
}
