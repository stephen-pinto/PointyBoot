using PointyBoot.Attributes;
using PointyBoot.Core.Context;
using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly IActivatorStore interContextSharedInfo;

        public IDIContext ContextInfo { get; private set; }

        public IOCProvider(IDIContext contextInfo)
        {
            ContextInfo = contextInfo;
            interContextSharedInfo = PBServicesFactory.GetGlobalActivatorCache();
        }

        /// <summary>
        /// New instantiator of generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentContext"></param>
        /// <returns></returns>
        public T New<T>(IDIContext currentContext = null)
        {
            return (T)New(typeof(T), currentContext);
        }

        /// <summary>
        /// Creates new instance by Type. If for the given context (or default context) the initializer is specified then use that.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contextInfo"></param>
        /// <returns></returns>
        public object New(Type type, IDIContext contextInfo = null)
        {
            contextInfo ??= ContextInfo;
            var solidType = contextInfo.TypeMapping.ContainsKey(type) ? contextInfo.TypeMapping[type] : null;

            //Check if we already have a singleton stored of this type (or solid type)
            if (contextInfo.SingletonStore.ContainsKey(type))
                return contextInfo.SingletonStore[type];
            else if (solidType != null && contextInfo.SingletonStore.ContainsKey(type))
                return contextInfo.SingletonStore[solidType];

            //Check if we already have a factory function for this type (or solid type)
            if (contextInfo.FactoryFunctionStore.ContainsKey(type))
                return contextInfo.FactoryFunctionStore[type];
            else if (solidType != null && contextInfo.FactoryFunctionStore.ContainsKey(solidType))
                return contextInfo.FactoryFunctionStore[solidType];

            //Instantiate
            object instance = null;

            //If there is factory defined for this interface/class then use that else use regular
            if (contextInfo.FactoryFunctionStore.ContainsKey(type))
            {
                instance = contextInfo.FactoryFunctionStore[type].Invoke();
            }
            else if (solidType != null && contextInfo.FactoryFunctionStore.ContainsKey(solidType))
            {
                instance = contextInfo.FactoryFunctionStore[solidType].Invoke();
            }
            else
            {
                //If solid type available then we wont be able to instantiate with 'type' anyway
                //Use 'solidType' then instead otherwise use the 'type'
                if (solidType != null)
                    instance = Instantiate(solidType);
                else
                    instance = Instantiate(type);
            }

            //If we were unable to find an instantiator then
            if (instance == null)
                throw new TypeAccessException($"Cannot instantiate type {type} as no factory or solid type defined.");

            //Set properties
            Wire(ref instance, type);

            return instance;
        }

        /// <summary>
        /// We wire the instances with properties or functions here.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="type"></param>
        /// <param name="contextInfo"></param>
        public void Wire(ref object instance, Type type, IDIContext contextInfo = null)
        {
            contextInfo = contextInfo ?? ContextInfo;
            var properties = type.GetProperties().Where(prop => prop.IsDefined(typeof(Autowired), false));

            if (!properties.Any())
                return;

            foreach (var prop in properties)
            {
                //var attributes = prop.GetCustomAttributes(typeof(Autowired), true).FirstOrDefault() as Autowired;
                prop.SetValue(instance, New(prop.PropertyType, contextInfo));
            }
        }

        /// <summary>
        /// Generates an activator function for the type and stores that for future use. 
        /// Thus better for performance for instances of the same type
        /// </summary>
        /// <param name="type">Type of object to instantiate</param>
        /// <param name="contextInfo">Context data to refer</param>
        /// <returns></returns>
        public object Instantiate(Type type, IDIContext contextInfo = null)
        {
            contextInfo ??= ContextInfo;
            var constructor = GetInitializableConstructor(type);

            if (constructor == null)
                throw new ArgumentException("No suitable constructor found. Need either Autowired or Default constructor present.");

            var parameters = constructor.GetParameters();

            ObjectActivator ObjActivatorForType;
            if (interContextSharedInfo.ObjectActivators.ContainsKey(type))
            {
                ObjActivatorForType = interContextSharedInfo.ObjectActivators[type];
            }
            else
            {
                //TODO: Check if we can use BuildPrimitiveActivator here for parameterless constructor
                ObjActivatorForType = BuildObjectActivator(constructor, parameters);
                interContextSharedInfo.ObjectActivators.Add(type, ObjActivatorForType);
            }

            //If no parameterized constructor then return plain instance
            if (!parameters.Any())
            {
                return ObjActivatorForType();
            }

            //Get primitive values if defined
            var primVals = constructor.GetCustomAttribute<Autowired>().PrimitiveTypeValues;

            //Else get instance of dependent instances
            object[] paramInstances = SetParameters(contextInfo, parameters, primVals);

            return ObjActivatorForType(paramInstances);
        }

        /// <summary>
        /// Uses a basic activation based on constructor discovered.
        /// </summary>
        /// <param name="type">Type of object to instantiate</param>
        /// <param name="contextInfo">Context data to refer</param>
        /// <returns></returns>
        public object InstantiateBasic(Type type, IDIContext contextInfo = null)
        {
            contextInfo ??= ContextInfo;
            var constructor = GetInitializableConstructor(type);

            if (constructor == null)
                throw new ArgumentException("Np suitable constructor found. Need either Autowired or Default constructor");

            var parameters = constructor.GetParameters();

            //If no parameterized constructor then return plain instance
            if (!parameters.Any())
                return Activator.CreateInstance(type);

            //Get primitive values if defined
            var primVals = constructor.GetCustomAttribute<Autowired>().PrimitiveTypeValues;

            //Else get instance of dependent instances
            object[] paramInstances = SetParameters(contextInfo, parameters, primVals);

            return Activator.CreateInstance(type, paramInstances);
        }

        #region Private functions

        /// <summary>
        /// Set parameters when wiring.
        /// </summary>
        /// <param name="contextInfo"></param>
        /// <param name="parameters"></param>
        /// <param name="primVals"></param>
        /// <returns></returns>
        private object[] SetParameters(IDIContext contextInfo, ParameterInfo[] parameters, object[] primVals)
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
                    if (primVals != null && primVals.Length >= primValIndex)
                        paramInstances[i] = primVals[primValIndex++];
                    else
                        paramInstances[i] = Activator.CreateInstance(parmType);
                }
                else
                {
                    paramInstances[i] = New(parmType, contextInfo);
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

        /// <summary>
        /// Builds a argument based activator
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="paramsInfo"></param>
        /// <returns></returns>
        private ObjectActivator BuildObjectActivator(ConstructorInfo ctor, ParameterInfo[] paramsInfo)
        {
            //Create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]));
            Expression[] argsExp = new Expression[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
                argsExp[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), paramsInfo[i].ParameterType);

            //Make a NewExpression that calls the ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //Create a lambda with the NewExpression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //Compile it
            ObjectActivator compiledActivator = (ObjectActivator)lambda.Compile();
            return compiledActivator;
        }

        #endregion
    }
}