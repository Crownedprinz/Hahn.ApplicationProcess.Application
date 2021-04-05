using System;
using HAF.Domain;

namespace  HAF.Connectors.Dropscan
{
    public class AddRecurringImportJob : IApplicationStartUpHandler
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public AddRecurringImportJob(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
        }

        public void Handle()
        {
            _recurringJobManager.AddOrUpdate<Connector>("dropscan-hourly-import", x => x.ImportNewMailings(), Cron.Hourly());
        }
    }
}