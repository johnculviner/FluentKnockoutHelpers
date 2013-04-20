using System.Web.Http;
using Ninject.Extensions.Conventions;
using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using SurveyApp.Model.Database;
using SurveyApp.Web.App_Start;
using SurveyApp.Web.App_Start.MvcApplication.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectConfig), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectConfig), "Stop")]

namespace SurveyApp.Web.App_Start
{
    public static class NinjectConfig 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            //for web api
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x => x.FromAssembliesMatching("SurveyApp.*").SelectAllClasses().BindAllInterfaces());
            kernel.Load<RavenDbNinjectModule>();
        }        
    }
}
