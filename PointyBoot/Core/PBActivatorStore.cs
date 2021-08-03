using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace PointyBoot.Core
{   
    public class PBActivatorStore : IActivatorStore
    {
        public Dictionary<Type, ObjectActivator> ObjectActivators { get; set; }

        private PBActivatorStore()
        {
            ObjectActivators = new Dictionary<Type, ObjectActivator>();
        }        
    }
}
