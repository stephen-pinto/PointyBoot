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
            interContextSharedInfo = PBServicesFactory.GetActivatorStore();
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

            //Check if we already have a singleton stored of this type
            if (contextInfo.SingletonStore.ContainsKey(type))
                return contextInfo.SingletonStore[type];

            //Check if we already have a factory function for this type
            if (contextInfo.FactoryFunctionStore.ContainsKey(type))
                return contextInfo.FactoryFunctionStore[type];

            //Instantiate
            object instance = null;

            //If there is factory defined for this class then use that else use regular
            if (!contextInfo.FactoryFunctionStore.ContainsKey(type))
                instance = Instantiate2(type);
            else
                instance = contextInfo.FactoryFunctionStore[type].Invoke();

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
                var attributes = prop.GetCustomAttributes(typeof(Autowired), true).FirstOrDefault() as Autowired;
                prop.SetValue(instance, New(prop.PropertyType, contextInfo));
            }
        }

        /// <summary>
        /// Uses a basic activation based on constructor discovered.
        /// </summary>
        /// <param name="type">Type of object to instantiate</param>
        /// <param name="contextInfo">Context data to refer</param>
        /// <returns></returns>
        public object Instantiate(Type type, IDIContext contextInfo = null)
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

        /// <summary>
        /// Generates an activator function for the type and stores that for future use. 
        /// Thus better for performance for instances of the same type
        /// </summary>
        /// <param name="type">Type of object to instantiate</param>
        /// <param name="contextInfo">Context data to refer</param>
        /// <returns></returns>
        public object Instantiate2(Type type, IDIContext contextInfo = null)
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
        /// New instantiator of generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentContext"></param>
        /// <returns></returns>
        public T New<T>(IDIContext currentContext = null)
        {
            return (T)New(typeof(T), currentContext);
        }

        #region Private functions
        
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

        /// <summary>
        /// Builds an argumentless activator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Still determining it's importance")]
        private SpecificObjectActivator<T> BuildPrimitiveActivator<T>()
        {
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            //Make a NewExpression that calls the ctor with the args we just created
            NewExpression newExp = Expression.New(typeof(T));

            //Create a lambda with the NewExpression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(SpecificObjectActivator<T>), newExp, param);

            //Compile it
            SpecificObjectActivator<T> compiled = (SpecificObjectActivator<T>)lambda.Compile();
            return compiled;
        }

        #endregion

        #region Undecided code

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Still determining it's importance")]
        private object CallConstructor(Type type, params object[] argValues)
        {
            var ctor = type.GetConstructors().First();
            ParameterInfo[] parameterInfo = ctor.GetParameters();
            IEnumerable<Type> parameterTypes = parameterInfo.Select(p => p.ParameterType);

            Expression[] args = new Expression[parameterInfo.Length];
            ParameterExpression param = Expression.Parameter(typeof(object[]));

            for (int i = 0; i < parameterInfo.Length; i++)
                args[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameterInfo[i].ParameterType);

            var lambda = Expression.Lambda(typeof(ObjectActivator), Expression.Convert(Expression.New(ctor, args), ctor.DeclaringType), param);
            ObjectActivator compiledActivator = (ObjectActivator)lambda.Compile();

            return compiledActivator(argValues);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Still determining it's importance")]
        private MethodInfo BuildGenericMethod(string methodName, Type context, Type genType)
        {
            MethodInfo method = context.GetMethod(methodName);
            MethodInfo generic = method.MakeGenericMethod(genType);
            return generic;
        }

        //private void InvokeCon(ConstructorInfo constructorInfo, ParameterInfo[] pi, object[] p, Type type)
        //{
        //    var mi = typeof(IOCProvider).GetMethod(nameof(GetObjectActivator2));
        //    mi = mi.MakeGenericMethod(type);
        //    var obj = mi.Invoke(constructorInfo, pi);
        //    Type ctype = typeof(ObjectActivator2<>);
        //    Type gtype = ctype.MakeGenericType(type);            
        //}
        #endregion
    }
}