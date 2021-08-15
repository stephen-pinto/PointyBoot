using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PointyBoot.Core
{
    internal static class IOCHelper
    {
        /// <summary>
        /// Builds a argument based activator
        /// </summary>
        /// <param name="ctor"></param>
        /// <param name="paramsInfo"></param>
        /// <returns></returns>
        public static GenericActivator BuildObjectActivator(ConstructorInfo ctor, ParameterInfo[] paramsInfo)
        {
            //Create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]));
            Expression[] argsExp = new Expression[paramsInfo.Length];

            //Create the array indexing expression for all the parameters
            for (int i = 0; i < paramsInfo.Length; i++)
                argsExp[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), paramsInfo[i].ParameterType);

            //Make a NewExpression that calls the ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //Create a lambda with the NewExpression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(GenericActivator), newExp, param);

            //Compile it
            GenericActivator compiledActivator = (GenericActivator)lambda.Compile();
            return compiledActivator;
        }

        public static StronglyTypedActivator<T> BuildPrimitiveActivator<T>()
        {
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            //Make a NewExpression that calls the ctor with the args we just created
            NewExpression newExp = Expression.New(typeof(T));

            //Create a lambda with the NewExpression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(StronglyTypedActivator<T>), newExp, param);

            //Compile it
            StronglyTypedActivator<T> compiled = (StronglyTypedActivator<T>)lambda.Compile();
            return compiled;
        }

        public static object CallConstructor(Type type, params object[] argValues)
        {
            var ctor = type.GetConstructors().First();
            ParameterInfo[] parameterInfo = ctor.GetParameters();
            IEnumerable<Type> parameterTypes = parameterInfo.Select(p => p.ParameterType);

            Expression[] args = new Expression[parameterInfo.Length];
            ParameterExpression param = Expression.Parameter(typeof(object[]));

            for (int i = 0; i < parameterInfo.Length; i++)
                args[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameterInfo[i].ParameterType);

            var lambda = Expression.Lambda(typeof(GenericActivator), Expression.Convert(Expression.New(ctor, args), ctor.DeclaringType), param);
            GenericActivator compiledActivator = (GenericActivator)lambda.Compile();

            return compiledActivator(argValues);
        }

        public static MethodInfo BuildGenericMethod(string methodName, Type context, Type genType)
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
    }
}
