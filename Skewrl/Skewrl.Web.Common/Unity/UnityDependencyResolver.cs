using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web;

namespace Skewrl.Web.Common.Unity
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        IUnityContainer container;
        public UnityDependencyResolver(IUnityContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                if (typeof(IController).IsAssignableFrom(serviceType))
                    return container.Resolve(serviceType);
                else
                    return container.IsRegistered(serviceType) ? container.Resolve(serviceType) : null;
            }
            catch (HttpException ex)
            {
                //@TODO

                //if (ex.GetHttpCode() == (int)HttpStatusCode.NotFound)
                //{
                //    ErrorController errorController = GetErrorController();
                //    errorController.InvokeHttp404(errorController.HttpContext);

                //    return errorController;
                //}
                //else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                if (typeof(IController).IsAssignableFrom(serviceType))
                    return container.ResolveAll(serviceType);
                else
                    return container.IsRegistered(serviceType) ? container.ResolveAll(serviceType) : new List<object>();
            }
            catch (HttpException ex)
            {
                //@TODO

                //if (ex.GetHttpCode() == (int)HttpStatusCode.NotFound)
                //{
                //    ErrorController errorController = GetErrorController();
                //    errorController.InvokeHttp404(errorController.HttpContext);

                //    return new List<object> { errorController };
                //}
                //else
                    return null;
            }
            catch
            {
                return new List<object>();
            }
        }

        //private ErrorController GetErrorController()
        //{
        //    return DependencyResolver.Current.GetService(typeof(ErrorController)) as ErrorController;
        //}
    }
}
