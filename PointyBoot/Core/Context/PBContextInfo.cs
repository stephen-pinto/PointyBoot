using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Context
{
    public class PBContextInfo
    {
        public Dictionary<Type, Type> TypeMapping { get; private set; }

        public Dictionary<Type, object> SingletonStore { get; private set; }

        public Dictionary<Type, Func<object>> FactoryFunctionStore { get; private set; }

        public PBContextInfo()
        {
            FactoryFunctionStore = new Dictionary<Type, Func<object>>();
            SingletonStore = new Dictionary<Type, object>();
        }
    }
}
