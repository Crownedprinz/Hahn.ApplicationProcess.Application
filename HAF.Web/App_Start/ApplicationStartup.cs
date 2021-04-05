using System.Web.Http;
using Crowd.Rest.Client;
using HAF.Domain;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace HAF.Web
{
    public static class ApplicationStartup
    {
        public static void Execute(HttpConfiguration config)
        {
            CompositionRoot.ApplicationStartup.BeforeInternalConfiguration = c =>
            {
                c.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
                c.RegisterSingleton(config);
            };
            CompositionRoot.ApplicationStartup.AfterInternalConfiguration = AfterInternalConfiguration;
            CompositionRoot.ApplicationStartup.AfterVerification = AfterVerification;
            CompositionRoot.ApplicationStartup.Execute();
        }

        private static void AfterInternalConfiguration(Container container)
        {
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            //container.RegisterSingleton(
            //    new CrowdRestClientManager("https://crowd.sovarto.com", "simple-dms.test", "&Y#R1c6r12&Y7L9noLq9"));
            //container.Options.AllowOverridingRegistrations = true;
            //container.RegisterSingleton<IRecurringJobManager, HangfireRecurringJobManager>();
            //container.Options.AllowOverridingRegistrations = false;
            //HangfireBootstrapper.Instance.Start("SimpleDMSConnectionString", container);
        }

        private static void AfterVerification(Container container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}