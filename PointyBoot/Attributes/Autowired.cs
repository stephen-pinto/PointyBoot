using System;

namespace PointyBoot.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor)]
    public sealed class Autowired : Attribute
    {
        public Autowired()
        {}

        public Autowired(params object[] primitiveParamValues)
        {
            PrimitiveDefaults = primitiveParamValues;          
        }

        internal object[] PrimitiveDefaults { get; private set; }
    }
}
