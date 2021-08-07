using PointyBoot.Core.Models;
using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Interfaces
{
    public interface IActivatorStore
    {
        Dictionary<Type, PBObjectInfo> ObjectInfo { get; }
    }
}
