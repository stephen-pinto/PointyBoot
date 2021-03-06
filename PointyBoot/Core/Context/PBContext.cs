using PointyBoot.Attributes.Provider;
using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointyBoot.Core.Context
{
    public class PBContext : IDIContext
    {
        private PBContextInfo contextInfo;

        public IReadOnlyDictionary<Type, object> SingletonStore => contextInfo.SingletonStore;

        public IReadOnlyDictionary<Type, Func<object>> FactoryFunctionStore => contextInfo.FactoryFunctionStore;

        public IReadOnlyDictionary<Type, Type> TypeMapping => contextInfo.TypeMapping;

        internal PBContext(PBContextInfo contextInfo)
        {
            this.contextInfo = contextInfo;            
        }

        public void AddFactoryFunction(Type type, Func<object> func)
        {
            contextInfo.FactoryFunctionStore.Add(type, func);
        }

        public void AddTypeMapping(Type intfType, Type actType)
        {
            contextInfo.TypeMapping.Add(intfType, actType);
        }

        public void AddSingleton(Type type, object instance)
        {
            contextInfo.SingletonStore.Add(type, instance);
        }

        public void LoadComponentFactory<T>(T instance)
        {
            var targetType = typeof(T);

            //Get all the properties with this attribute
            var properties = targetType.GetProperties().Where(prop => prop.IsDefined(typeof(PointyComponentProviderProp), false));

            //FIXME: Provide handling for properties
            if (properties.Any())
                throw new NotImplementedException();

            //Get all the functions with this attribute
            var functions = targetType.GetMethods().Where(meth => meth.IsDefined(typeof(PointyComponentProviderFunc), false));

            if (functions.Any())
            {
                for (int i = 0; i < functions.Count(); i++)
                {
                    var func = functions.ElementAt(i);

                    //If there is already a factory function for this type then throw exception
                    if (contextInfo.FactoryFunctionStore.ContainsKey(func.ReturnType))
                        throw new InvalidOperationException("Only one factory function can be supported per type");

                    //Get attribute object to get parameters for this function
                    var attribute = func.GetCustomAttributes(typeof(PointyComponentProviderFunc), true).FirstOrDefault() as PointyComponentProviderFunc;

                    //Save a lambda function to be able to invoke later
                    contextInfo.FactoryFunctionStore.Add(func.ReturnType, () =>
                    {
                        var parameters = attribute.Parameters;
                        var obj = instance;
                        return func.Invoke(obj, parameters);
                    });
                }
            }
        }

        public void Dispose()
        {
            contextInfo.FactoryFunctionStore.Clear();
            contextInfo.SingletonStore.Clear();
        }
    }
}
