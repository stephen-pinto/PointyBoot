using PointyBoot.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace PointyBoot.Core.Context
{
    public class PBContext : IDIContext
    {   
        private readonly IOCProvider instanceProvider;
        private readonly PBContextHelper contextHelper;
        private PBContextInfo contextInfo;

        public IReadOnlyDictionary<Type, object> SingletonStore => contextInfo.SingletonStore;

        public IReadOnlyDictionary<Type, Func<object>> FactoryFunctionStore => contextInfo.FactoryFunctionStore;

        public IReadOnlyDictionary<Type, Type> TypeMapping => contextInfo.TypeMapping;

        internal PBContext(PBContextInfo contextInfo)
        {
            this.contextInfo = contextInfo;
            instanceProvider = PBServicesFactory.GetIOCProvider();
            contextHelper = PBContextFactory.GetContextHelper();
        }

        public T Get<T>()
        {
            return instanceProvider.New<T>();
        }

        public void RegisterComponentFactory<T>(T obj)
            where T : class
        {
            contextHelper.LoadComponentFactory(contextInfo, obj);
        }

        public void RegisterFactory<T>(Func<T> factory)
            where T : class
        {
            contextInfo.FactoryFunctionStore.Add(typeof(T), factory);
        }

        public void AddMapping<IntfType, ActType>() where ActType : IntfType
        {
            if (!contextInfo.TypeMapping.ContainsKey(typeof(IntfType)))
                contextInfo.TypeMapping.Add(typeof(IntfType), typeof(ActType));
            else
                throw new ArgumentException($"Mapping for type {typeof(IntfType).Name} is already defined");
        }

        public void AddSingleton<T>()
        {
            contextInfo.SingletonStore.Add(typeof(T), Get<T>());
        }

        public void AddSingleton<T>(object instance)
        {
            contextInfo.SingletonStore.Add(typeof(T), instance);
        }

        public void AddSingleton<T>(Func<T> instantiatorFunction)
        {
            if (instantiatorFunction is null)
                throw new ArgumentNullException(nameof(instantiatorFunction));

            contextInfo.SingletonStore.Add(typeof(T), instantiatorFunction);
        }

        public void Dispose()
        {
            contextInfo.FactoryFunctionStore.Clear();
            contextInfo.SingletonStore.Clear();
        }
    }
}
