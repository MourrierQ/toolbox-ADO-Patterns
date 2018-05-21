using Patterns.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Locator
{
    public abstract class LocatorBase
    {
        private ISimpleIOC _container;

        public ISimpleIOC Container
        {
            get
            {
                return _container;
            }

            set
            {
                _container = value;
            }
        }

        protected LocatorBase() : this(new SimpleIOC())
        {

        }

        protected LocatorBase(ISimpleIOC Container)
        {
            this.Container = Container;
        }

    }

    //Exemple of a Service Locator
    //public class ServiceLocator : LocatorBase
    //{
    //    private ServiceLocator _instance;

    //    public ServiceLocator Instance
    //    {
    //        get
    //        {
    //            return _instance ?? (_instance = new ServiceLocator());
    //        }
    //    }

    //    private ServiceLocator()
    //    {
    //        //Container.Register<ContactService>();
    //    }

    //    public ContactService Contact
    //    {
    //        get
    //        {
    //            return Container.GetInstance<ContactService>();
    //        }
    //    }

    //}


}
