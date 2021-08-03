using System;
using System.Collections.Generic;

namespace PointyBoot.Core
{
    public class PBContextInfo
    {
        public PBContextInfo()
        {
            FactoryFunctionStore = new Dictionary<Type, Func<object>>();
            SingletonStore = new Dictionary<Type, object>();
        }

        public Dictionary<Type, Func<object>> FactoryFunctionStore { get; internal set; }

        public Dictionary<Type, object> SingletonStore { get; internal set; }
    }
}
