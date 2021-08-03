using PointyBoot.BaseStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointyBoot.Attributes.Provider
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class PointyBindingProvider : Attribute
    {
    }
}
