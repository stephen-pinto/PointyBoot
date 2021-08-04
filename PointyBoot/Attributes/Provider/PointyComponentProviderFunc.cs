using System;

namespace PointyBoot.Attributes.Provider
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public sealed class PointyComponentProviderFunc : Attribute
    {
        public PointyComponentProviderFunc()
        {}

        public PointyComponentProviderFunc(params object[] parameters)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; private set; }
    }   
}
