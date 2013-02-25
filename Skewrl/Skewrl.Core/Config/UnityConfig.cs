using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using System.Threading;

namespace Skewrl.Core.Config
{
    public sealed class UnityConfig
    {
        private static UnityConfig _Instance = null;
        private IUnityContainer _Container;
        public UnityServiceLocator _Locator;

        private UnityConfig()
        {
            _Container = new UnityContainer();
            _Locator = new UnityServiceLocator(_Container);
            ServiceLocator.SetLocatorProvider(() => _Locator);
        }

        public static UnityConfig Instance
        {
            get
            {
                if (_Instance != null) return _Instance;

                UnityConfig tempObj = new UnityConfig();
                Interlocked.CompareExchange(ref _Instance, tempObj, null);

                return _Instance;
            }
        }

        public IUnityContainer Container
        {
            get { return _Container; }
        }

        public T Resolve<T>()
        {
            return _Locator.GetInstance<T>();
        }

        public object Resolve(Type t)
        {
            return _Locator.GetInstance(t);
        }
    }
}
