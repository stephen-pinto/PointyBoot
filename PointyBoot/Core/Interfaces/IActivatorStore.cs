using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Interfaces
{
    public interface IActivatorStore
    {
        Dictionary<Type, ObjectActivator> ObjectActivators { get; }
    }
}
