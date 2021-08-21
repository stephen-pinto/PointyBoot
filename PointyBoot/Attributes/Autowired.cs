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

        /// <summary>
        /// Default values for primitive types
        /// </summary>
        internal object[] PrimitiveDefaults { get; private set; }
    }
}
