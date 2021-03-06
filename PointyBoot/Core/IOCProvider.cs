using PointyBoot.Attributes;
using PointyBoot.Core.Interfaces;
using PointyBoot.Core.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PointyBoot.Core
{
    /// <summary>
    /// This is the main class which provides mapping and instantiation based on attributes
    /// </summary>
    public class IOCProvider
    {
        private readonly IActivatorStore glblSharedInfo;

        public IOCProvider()
        {
            glblSharedInfo = PBServicesFactory.GetGlobalActivatorCache();
        }

        /// <summary>
        /// New instantiator of generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public T New<T>(IDIContext context)
        {
            return (T)New(context, typeof(T));
        }

        /// <summary>
        /// Creates new instance by Type. If for the given context (or default context) the initializer is specified then use that.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object New(IDIContext context, Type type)
        {
            var solidType = context.TypeMapping.ContainsKey(type) ? context.TypeMapping[type] : null;

            //Check if we already have a singleton stored of this type (or solid type)
            //TODO: Check we also need to wire this one
            if (context.SingletonStore.ContainsKey(type))
                return context.SingletonStore[type];
            else if (solidType != null && context.SingletonStore.ContainsKey(type))
                return context.SingletonStore[solidType];

            if (!glblSharedInfo.ObjectInfo.ContainsKey(type))
                glblSharedInfo.ObjectInfo.Add(type, new PBObjectInfo(type));

            //Instantiate
            object instance = null;

            //If there is factory defined for this interface/class then use that else use regular
            if (context.FactoryFunctionStore.ContainsKey(type))
            {
                instance = context.FactoryFunctionStore[type].Invoke();
            }
            else if (solidType != null && context.FactoryFunctionStore.ContainsKey(solidType))
            {
                instance = context.FactoryFunctionStore[solidType].Invoke();
            }
            else
            {
                //If solid type available then we wont be able to instantiate with 'type' anyway
                //Use 'solidType' then instead otherwise use the 'type'
                if (solidType != null)
                    instance = Instantiate(context, solidType);
                else
                    instance = Instantiate(context, type);
            }

            //If we were unable to find an instantiator then
            if (instance == null)
                throw new TypeAccessException($"Cannot instantiate type {type} as no factory or solid type defined.");

            //Set properties            
            Wire(context, ref instance, type);

            return instance;
        }

        /// <summary>
        /// We wire the instances with properties or functions here.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="instance"></param>
        /// <param name="type"></param>
        public void Wire(IDIContext context, ref object instance, Type type)
        {
            var autowiredProps = glblSharedInfo.ObjectInfo[type].AutowiredProperties;

            if (!autowiredProps.Any())
                return;

            foreach (var prop in autowiredProps)
            {
                //var attributes = prop.GetCustomAttributes(typeof(Autowired), true).FirstOrDefault() as Autowired;
                prop.SetValue(instance, New(context, prop.PropertyType));
            }
        }

        /// <summary>
        /// Generates an activator function for the type and stores that for future use. 
        /// Thus better for performance for instances of the same type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Instantiate(IDIContext context, Type type)
        {
            var objInfo = glblSharedInfo.ObjectInfo[type];
            var constructor = objInfo.CallableConstructor;

            if (constructor == null)
                throw new ArgumentException("No suitable constructor found. Need either Autowired or Default constructor present.");

            var parameters = objInfo.ConstructorParams;

            GenericActivator objActivator;
            if (objInfo.Activator != null)
            {
                objActivator = objInfo.Activator;
            }
            else
            {
                //TODO: Check if we can use BuildPrimitiveActivator here for parameterless constructor
                objActivator = IOCHelper.BuildObjectActivator(constructor, parameters);
                objInfo.Activator = objActivator;
            }

            //If no parameterized constructor then return plain instance
            if (!parameters.Any())
                return objActivator();
            
            //Get primitive values if defined
            var primitiveDefaults = objInfo.ConstructorAttribute.PrimitiveDefaults;

            //Else get instance of dependent instances
            object[] paramInstances = SetParameters(context, parameters, primitiveDefaults);

            return objActivator(paramInstances);
        }

        /// <summary>
        /// Uses a basic activation based on constructor discovered.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object InstantiateBasic(IDIContext context, Type type)
        {
            var constructor = GetInitializableConstructor(type);

            if (constructor == null)
                throw new ArgumentException("Np suitable constructor found. Need either Autowired or Default constructor");

            var parameters = constructor.GetParameters();

            //If no parameterized constructor then return plain instance
            if (!parameters.Any())
                return Activator.CreateInstance(type);

            //Get primitive values if defined
            var primVals = constructor.GetCustomAttribute<Autowired>().PrimitiveDefaults;

            //Else get instance of dependent instances
            object[] paramInstances = SetParameters(context, parameters, primVals);

            return Activator.CreateInstance(type, paramInstances);
        }

        #region Private functions

        /// <summary>
        /// Set parameters when wiring.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <param name="primitiveDefaults"></param>
        /// <returns></returns>
        private object[] SetParameters(IDIContext context, ParameterInfo[] parameters, object[] primitiveDefaults)
        {
            //Else get instance of dependent instances
            object[] paramInstances = new object[parameters.Length];

            //Get primitive values if defined
            var primValIndex = 0;

            //Prepare a list of parameters and instantiate them if necessary
            for (int i = 0; i < parameters.Length; i++)
            {
                var parmType = parameters[i].ParameterType;

                if (parmType.IsPrimitive)
                {
                    //If it is a primitive type then instantiate with regular method activator
                    if (primitiveDefaults != null && primitiveDefaults.Length >= primValIndex)
                        paramInstances[i] = primitiveDefaults[primValIndex++];
                    else
                        paramInstances[i] = Activator.CreateInstance(parmType);
                }
                else
                {
                    //Else use the recursive activation process
                    paramInstances[i] = New(context, parmType);
                }
            }

            return paramInstances;
        }

        /// <summary>
        /// Get initialzable constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ConstructorInfo GetInitializableConstructor(Type type)
        {
            var constructors = type.GetConstructors();
            ConstructorInfo defaultConstructor = null;

            //Find constructor with Autowired attribute if not find default constructor
            defaultConstructor = constructors.Where(c => c.IsDefined(typeof(Autowired)) || c.GetParameters().Length == 0).FirstOrDefault();

            return defaultConstructor;
        }

        #endregion
    }
}