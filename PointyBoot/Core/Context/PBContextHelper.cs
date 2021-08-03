using PointyBoot.Attributes.Provider;
using System;
using System.Linq;

namespace PointyBoot.Core.Context
{
    public class PBContextHelper
    {
        public void LoadComponentFactory<T>(ref PBContextInfo context, T instance)
        {
            var targetType = typeof(T);
            
            //Get all the properties with this attribute
            var properties = targetType.GetProperties().Where(prop => prop.IsDefined(typeof(PointyComponentProviderProp), false));
            
            //FIXME: Provide handling for properties
            if(properties.Any())
                throw new NotImplementedException();
            
            //Get all the functions with this attribute
            var functions = targetType.GetMethods().Where(meth => meth.IsDefined(typeof(PointyComponentProviderFunc), false));
            
            if(functions.Any())
            {
                for (int i = 0; i < functions.Count(); i++)
                {
                    var func = functions.ElementAt(i);

                    //If there is already a factory function for this type then throw exception
                    if (context.FactoryFunctionStore.ContainsKey(func.ReturnType))
                        throw new InvalidOperationException("Only one factory function can be supported per type");

                    //Get attribute object to get parameters for this function
                    var attribute = func.GetCustomAttributes(typeof(PointyComponentProviderFunc), true).FirstOrDefault() as PointyComponentProviderFunc;
                    
                    //Save a lambda function to be able to invoke later
                    context.FactoryFunctionStore.Add(func.ReturnType, () =>
                    {
                        var parameters = attribute.Parameters;
                        var obj = instance;
                        return func.Invoke(obj, parameters);
                    });
                }
            }
        }
    }
}
