﻿using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Context
{
    public class PBContextInfo
    {
        public Dictionary<Type, object> SingletonStore { get; internal set; }

        public Dictionary<Type, Func<object>> FactoryFunctionStore { get; internal set; }

        public PBContextInfo()
        {
            FactoryFunctionStore = new Dictionary<Type, Func<object>>();
            SingletonStore = new Dictionary<Type, object>();
        }
    }
}