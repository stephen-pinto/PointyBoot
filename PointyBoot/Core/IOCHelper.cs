using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PointyBoot.Core
{
    public static class IOCHelper
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

        /// <summary>
        /// Builds a primitive activator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Calls the constructor by type and arguments
        /// </summary>
        /// <param name="type"></param>
        /// <param name="argValues"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Builds generic method using expression
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="context"></param>
        /// <param name="genType"></param>
        /// <returns></returns>
        public static MethodInfo BuildGenericMethod(string methodName, Type context, Type genType)
        {
            MethodInfo method = context.GetMethod(methodName);
            MethodInfo generic = method.MakeGenericMethod(genType);
            return generic;
        }

        /// <summary>
        /// This function builds a simple property setting lamda function for wiring.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        public static LambdaExpression BuildPropertySetterFunction(Type type, PropertyInfo[] propertyInfos)
        {
            //Define parameters for the lamda function
            var instance = Expression.Parameter(type, "obj");
            var propertyValues = Expression.Parameter(typeof(object[]), "instances");

            //TODO: Check if using this gives better performance
            //MemberAssignment[] assignments = new MemberAssignment[propertyInfos.Count()];
            //for (int i = 0; i < propertyInfos.Count(); i++)
            //    assignments[i] = Expression.Bind(propertyInfos[i], Expression.Convert(Expression.ArrayIndex(propertyValues, Expression.Constant(i)), propertyInfos[i].PropertyType));

            Expression[] assignmentExpressions = new Expression[propertyInfos.Length];

            //Prepare assigment expressions for the concerned properties
            for (int i = 0; i < propertyInfos.Length; i++)
                assignmentExpressions[i] = Expression.Assign(Expression.Property(instance, propertyInfos[i].Name), Expression.Convert(Expression.ArrayIndex(propertyValues, Expression.Constant(i)), propertyInfos[i].PropertyType));

            //Assemble the set of expressions as a block
            BlockExpression blockExpr = Expression.Block(assignmentExpressions);

            //Return the prepared lamda expression to be able to compile
            return Expression.Lambda(blockExpr, new ParameterExpression[] { instance, propertyValues });
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
