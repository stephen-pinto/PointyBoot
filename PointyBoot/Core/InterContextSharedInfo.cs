using System;
using System.Collections.Generic;

namespace PointyBoot.Core
{
    public delegate object ObjectActivator(params object[] args);
    public delegate T SpecificObjectActivator<T>(params object[] args);

    public class InterContextSharedInfo
    {
        private static Lazy<InterContextSharedInfo> defaultInstance = new Lazy<InterContextSharedInfo>(() => new InterContextSharedInfo());

        public Dictionary<Type, ObjectActivator> ObjectActivators { get; set; }

        private InterContextSharedInfo()
        {
            ObjectActivators = new Dictionary<Type, ObjectActivator>();
        }

        public static InterContextSharedInfo Instance
        {
            get => defaultInstance.Value;
        }
    }
}
