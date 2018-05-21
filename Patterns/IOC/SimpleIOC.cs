using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.IOC
{
    public class SimpleIOC : ISimpleIOC
    {
        private IDictionary<Type, object> _Instances;
        private IDictionary<Type, CtorInfo> _ConstructorInfos;
            
        public SimpleIOC()
        {
            _Instances = new Dictionary<Type, object>();
            _ConstructorInfos = new Dictionary<Type, CtorInfo>();
        }

        public void Register<TResource>()
        {
            Register<TResource>(null);
        }

        public void Register<TResource>(params object[] Parameters)
        {
            Type TTResource = typeof(TResource);
            _Instances.Add(TTResource, null);
            _ConstructorInfos.Add(TTResource, new CtorInfo(TTResource, Parameters));
        }

        public void Register<TInterface,TResource>()
            where TResource:TInterface
        {
            Register<TInterface,TResource>(null);
        }

        public void Register<TInterface,TResource>(params object[] Parameters)
            where TResource:TInterface
        {
            Type TTInterface = typeof(TInterface);

            if (!TTInterface.IsInterface)
                throw new ArgumentException("TIntreface must be an interface !");

            _Instances.Add(TTInterface, null);
            _ConstructorInfos.Add(TTInterface, new CtorInfo(typeof(TResource), Parameters));
        }

        public TResource GetInstance<TResource>()
        {
            Type TTResource = typeof(TResource);

            if (!_Instances.ContainsKey(TTResource))
                throw new ArgumentException("register your Resource before using it");

            if (_Instances[TTResource] == null)
                _Instances[TTResource] = Activator.CreateInstance(_ConstructorInfos[TTResource].TResource, _ConstructorInfos[TTResource].Parameters);

            return (TResource)_Instances[TTResource];
        }

        protected class CtorInfo
        {
            public Type TResource { get; private set; }
            public object[] Parameters { get; private set; }
            
            internal CtorInfo(Type TResource, object[] Parameters)
            {
                this.TResource = TResource;
                this.Parameters = Parameters;
            }
        }
    }
}
