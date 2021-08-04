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
            PrimitiveTypeValues = primitiveParamValues;          
        }

        internal object[] PrimitiveTypeValues { get; private set; }
    }
}
