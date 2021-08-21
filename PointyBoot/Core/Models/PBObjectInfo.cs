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

        public GenericActivator Activator { get; set; }

        public IReadOnlyList<PropertyInfo> AutowiredProperties { get => autowiredProperties; }

        public ConstructorInfo CallableConstructor { get; set; }

        public Autowired ConstructorAttribute { get; set; }

        public ParameterInfo[] ConstructorParams { get; set; }

        public Delegate PropertySetterDelegate { get; set; }

        public PBObjectInfo(Type type)
        {
            BaseType = type;
            LoadProps();
            LoadConstructors();
        }

        private void LoadProps()
        {
            var properties = BaseType.GetProperties().Where(prop => prop.IsDefined(typeof(Autowired), false));
            autowiredProperties.AddRange(properties);
        }

        private void LoadConstructors()
        {
            var constructors = BaseType.GetConstructors();

            //Find constructor with Autowired attribute if not find default constructor
            CallableConstructor = constructors.Where(c => c.IsDefined(typeof(Autowired)) || c.GetParameters().Length == 0).FirstOrDefault();

            if (CallableConstructor != null)
            {
                ConstructorAttribute = CallableConstructor.GetCustomAttribute<Autowired>();
                ConstructorParams = CallableConstructor.GetParameters();
            }
        }
    }
}
