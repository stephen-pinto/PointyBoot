using PointyBoot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PointyBoot.Core.Models
{
    public class PBObjectInfo
    {
        private List<PropertyInfo> autowiredProperties = new List<PropertyInfo>();

        public Type BaseType { get; private set; }

        public ObjectActivator Activator { get; set; }

        public IReadOnlyList<PropertyInfo> AutowiredProperties { get => autowiredProperties; }

        public PBObjectInfo(Type type)
        {
            BaseType = type;
            LoadProps();
        }

        private void LoadProps()
        {
            var properties = BaseType.GetProperties().Where(prop => prop.IsDefined(typeof(Autowired), false));
            autowiredProperties.AddRange(properties);
        }
    }
}
