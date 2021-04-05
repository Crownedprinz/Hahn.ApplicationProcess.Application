using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;

namespace Hahn.ApplicationProcess.February2021.Web.App_Start
{
    public static class ApplicationStartup
    {

        public static void Execute(HttpConfiguration config)
        {
            HAF.CompositionRoot.ApplicationStartup.BeforeInternalConfiguration = c =>
            {
                c.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
                c.RegisterSingleton(config);
            };
            HAF.CompositionRoot.ApplicationStartup.AfterInternalConfiguration = AfterInternalConfiguration;
            HAF.CompositionRoot.ApplicationStartup.AfterVerification = AfterVerification;
            HAF.CompositionRoot.ApplicationStartup.Execute();
        }

        private static void AfterInternalConfiguration(Container container)
        {
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            //container.RegisterSingleton(
            //    new CrowdRestClientManager("https://crowd.sovarto.com", "simple-dms.test", "&Y#R1c6r12&Y7L9noLq9"));
            container.Options.AllowOverridingRegistrations = true;
            //container.RegisterSingleton<IRecurringJobManager, HangfireRecurringJobManager>();
            container.Options.AllowOverridingRegistrations = false;
            //HangfireBootstrapper.Instance.Start("SimpleDMSConnectionString", container);
        }

        private static void AfterVerification(Container container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

    }
}