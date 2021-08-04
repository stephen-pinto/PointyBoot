using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace PointyBoot.Core
{   
    public class PBActivatorCache : IActivatorStore
    {
        public Dictionary<Type, ObjectActivator> ObjectActivators { get; set; }

        private PBActivatorCache()
        {
            ObjectActivators = new Dictionary<Type, ObjectActivator>();
        }        
    }
}
