using System;

namespace PointyBoot.Core
{
    public class PBContext : IServices, IContext
    {
        private readonly IOCProvider instanceProvider;
        private readonly PBContextHelper contextHelper;
        private PBContextInfo contextInfo;

        public PBContext(PBContextInfo contextInfo = null)
        {
            if(contextInfo == null)
                this.contextInfo = new PBContextInfo();

            instanceProvider = new IOCProvider(this.contextInfo);
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
    }
}
