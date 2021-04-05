using System.Web.Hosting;
using Hangfire;
using Hangfire.SimpleInjector;
using SimpleInjector;

namespace HAF.Web
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();
        private readonly object _lockObject = new object();
        private BackgroundJobServer _backgroundJobServer;
        private bool _started;

        private HangfireBootstrapper()
        {
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }

        public void Start(string connectionStringOrName, Container container)
        {
            lock (_lockObject)
            {
                if (_started)
                    return;
                _started = true;

                HostingEnvironment.RegisterObject(this);

                GlobalConfiguration.Configuration.UseSqlServerStorage(connectionStringOrName);
                GlobalConfiguration.Configuration.UseActivator(new SimpleInjectorJobActivator(container));

                _backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                _backgroundJobServer?.Dispose();

                HostingEnvironment.UnregisterObject(this);
            }
        }
    }
}