using System;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace SurveyApp.Web.App_Start
{
    namespace MvcApplication.App_Start
    {
        // Provides a Ninject implementation of IDependencyScope
        // which resolves services using the Ninject container.
        public class NinjectDependencyScope : IDependencyScope
        {
            IResolutionRoot resolver;

            public NinjectDependencyScope(IResolutionRoot resolver)
            {
                this.resolver = resolver;
            }

            public object GetService(Type serviceType)
            {
                if (resolver == null)
                    throw new ObjectDisposedException("this", "This scope has been disposed");

                return resolver.TryGet(serviceType);
            }

            public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
            {
                if (resolver == null)
                    throw new ObjectDisposedException("this", "This scope has been disposed");

                return resolver.GetAll(serviceType);
            }

            public void Dispose()
            {
                IDisposable disposable = resolver as IDisposable;
                if (disposable != null)
                    disposable.Dispose();

                resolver = null;
            }
        }

        // This class is the resolver, but it is also the global scope
        // so we derive from NinjectScope.
        public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
        {
            IKernel kernel;

            public NinjectDependencyResolver(IKernel kernel)
                : base(kernel)
            {
                this.kernel = kernel;
            }

            public IDependencyScope BeginScope()
            {
                return new NinjectDependencyScope(kernel.BeginBlock());
            }
        }
    }
}