using PointyBoot.Core.Interfaces;
using PointyBoot.Core.Models;
using System;
using System.Collections.Generic;

namespace PointyBoot.Core
{
    public class PBActivatorCache : IActivatorStore
    {
        /// TODO: Manage addition and removal of items using function and set the setters as private
        
        public Dictionary<Type, PBObjectInfo> ObjectInfo { get; set; }

        public PBActivatorCache()
        {
            ObjectInfo = new Dictionary<Type, PBObjectInfo>();
        }
    }
}
