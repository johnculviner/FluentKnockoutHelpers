using System;
using System.IO;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Database
{
    public class RavenDbNinjectModule : NinjectModule
    {
        public override void Load()
        {
            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(37928);
            var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                DataDirectory = "App_Data",
                //UseEmbeddedHttpServer = true
            };

            documentStore.Conventions.IdentityPartsSeparator = "-";

            var initializedStore = documentStore.Initialize();

            Bind<IDocumentStore>()
               .ToConstant(initializedStore)
               .InSingletonScope();


            //populate data
            DataInitializer.PopulateData(initializedStore);

            Bind<IDocumentSession>()
                .ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation(x =>
                {
                    if (x == null)
                        return;

                    x.SaveChanges();
                    x.Dispose();
                });
        }
    }
}



