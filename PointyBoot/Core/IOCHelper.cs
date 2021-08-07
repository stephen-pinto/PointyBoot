using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PointyBoot.Core
{
    public class IOCHelper
    {
        public SpecificObjectActivator<T> BuildPrimitiveActivator<T>()
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

        public object CallConstructor(Type type, params object[] argValues)
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

        public MethodInfo BuildGenericMethod(string methodName, Type context, Type genType)
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
