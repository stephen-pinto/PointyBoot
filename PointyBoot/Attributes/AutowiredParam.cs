using System;

namespace PointyBoot.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AutowiredParam : Attribute
    {
    }
}
