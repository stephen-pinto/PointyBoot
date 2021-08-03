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

        Dictionary<Type, object> IDIContext.SingletonStore
        {
            get
            {
                return contextInfo.SingletonStore;
            }
        }

        Dictionary<Type, Func<object>> IDIContext.FactoryFunctionStore
        {
            get
            {
                return contextInfo.FactoryFunctionStore;
            }
        }

        internal PBContext(PBContextInfo contextInfo)
        {
            this.contextInfo = contextInfo;
            instanceProvider = PBServicesFactory.GetIOCProvider();
            contextHelper = new PBContextHelper();
        }

        public static PBContext NewContext(PBContextInfo contextInfo)
        {
            return new PBContext(contextInfo);
        }

        public T Get<T>()
        {
            return instanceProvider.New<T>();
        }

        public void RegisterComponentFactory<T>(T obj)
        {
            contextHelper.LoadComponentFactory(ref contextInfo, obj);
        }

        public void RegisterFactory<T>(Func<T> factory)
        {
            contextInfo.FactoryFunctionStore.Add(typeof(T), () => factory());
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

            contextInfo.SingletonStore.Add(typeof(T), instantiatorFunction());
        }

        T IDIServices.Get<T>()
        {
            throw new NotImplementedException();
        }

        void IDIServices.RegisterComponentFactory<T>(T instance)
        {
            throw new NotImplementedException();
        }

        void IDIServices.AddSingleton<T>()
        {
            throw new NotImplementedException();
        }

        void IDIServices.AddSingleton<T>(object instance)
        {
            throw new NotImplementedException();
        }

        void IDIServices.AddSingleton<T>(Func<T> instantiatorFunction)
        {
            throw new NotImplementedException();
        }

        void IDIServices.RegisterFactory<T>(Func<T> factory)
        {
            throw new NotImplementedException();
        }
    }
}
