//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using TestApp.CommonTypes;

//namespace TestApp
//{
//    class OffTest
//    {
//        public void Test()
//        {
//            Test1();
//            //Test2();
//        }

//        private void Test1()
//        {
//            //object[] prm = new object[] { Activator.CreateInstance(typeof(CoordA)), Activator.CreateInstance(typeof(CoordB)), Activator.CreateInstance(typeof(int)) };
//            ConstructorInfo ctor = typeof(Area).GetConstructors().First();
//            var params1 = ctor.GetParameters();
//            ObjectActivator createdActivator = GetObjectActivator(ctor, params1);
//            var prm = SetParameters(params1, new object[] { 10000 });
//            var obj = createdActivator(prm);
//        }

//        private object GetInstance(Type type)
//        {
//            ConstructorInfo ctor = type.GetConstructors().First();
//            ObjectActivator createdActivator = GetObjectActivator(ctor, ctor.GetParameters());
//            return createdActivator();
//        }

//        private void Test2()
//        {
//            throw new NotImplementedException();
//        }

//        private object[] SetParameters(ParameterInfo[] parameters, object[] primVals)
//        {
//            //Else get instance of dependent instances
//            object[] paramInstances = new object[parameters.Length];

//            //Get primitive values if defined
//            var primValIndex = 0;

//            for (int i = 0; i < parameters.Length; i++)
//            {
//                if (parameters[i].ParameterType.IsPrimitive)
//                {
//                    if (primVals != null && primVals.Length >= primValIndex)
//                        paramInstances[i] = primVals[primValIndex++];
//                    else
//                        paramInstances[i] = GetInstance(parameters[i].ParameterType);
//                }
//                else
//                {
//                    paramInstances[i] = GetInstance(parameters[i].ParameterType);
//                }
//            }

//            return paramInstances;
//        }

//        public delegate object ObjectActivator(params object[] args);
//        public delegate object ObjectActivator2<T>(params object[] args);

//        private ObjectActivator GetObjectActivator(ConstructorInfo ctor, ParameterInfo[] paramsInfo)
//        {
//            Type type = ctor.DeclaringType;

//            //create a single param of type object[]
//            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

//            Expression[] argsExp = new Expression[paramsInfo.Length];

//            //pick each arg from the params array 
//            //and create a typed expression of them
//            for (int i = 0; i < paramsInfo.Length; i++)
//            {
//                Expression index = Expression.Constant(i);
//                Type paramType = paramsInfo[i].ParameterType;
//                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
//                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
//                argsExp[i] = paramCastExp;
//            }

//            //make a NewExpression that calls the
//            //ctor with the args we just created
//            NewExpression newExp = Expression.New(ctor, argsExp);

//            //create a lambda with the New
//            //Expression as body and our param object[] as arg
//            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

//            //compile it
//            ObjectActivator compiled = (ObjectActivator)lambda.Compile();
//            System.Diagnostics.Debug.WriteLine(lambda.ToString());
//            return compiled;
//        }

//        private ObjectActivator2<T> GetObjectActivator2<T>(ConstructorInfo ctor, ParameterInfo[] paramsInfo)
//        {
//            Type type = ctor.DeclaringType;

//            //create a single param of type object[]
//            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

//            Expression[] argsExp = new Expression[paramsInfo.Length];

//            //pick each arg from the params array 
//            //and create a typed expression of them
//            for (int i = 0; i < paramsInfo.Length; i++)
//            {
//                Expression index = Expression.Constant(i);
//                Type paramType = paramsInfo[i].ParameterType;
//                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
//                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
//                argsExp[i] = paramCastExp;
//            }

//            //make a NewExpression that calls the
//            //ctor with the args we just created
//            NewExpression newExp = Expression.New(ctor, argsExp);

//            //create a lambda with the New
//            //Expression as body and our param object[] as arg
//            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator2<T>), newExp, param);

//            //compile it
//            ObjectActivator2<T> compiled = (ObjectActivator2<T>)lambda.Compile();
//            return compiled;
//        }
//    }
//}
