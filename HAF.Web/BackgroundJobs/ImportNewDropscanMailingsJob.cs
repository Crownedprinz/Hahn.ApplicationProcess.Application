using System;
using System.Linq;
using HAF.Connectors.Dropscan;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace HAF.Web.BackgroundJobs
{
    public class ImportNewDropscanMailingsJob : BackgroundJobHandler<ImportNewDropscanMailingsResult>
    {
        private readonly Connector _connector;

        public ImportNewDropscanMailingsJob(
            Connector connector,
            ICommand<StartBackgroundJobProcessing> startJobProcessing,
            ICommand<SucceedBackgroundJob> succeedJob,
            ICommand<FailBackgroundJob> failJob)
            : base(startJobProcessing, succeedJob, failJob)
        {
            if (connector == null)
                throw new ArgumentNullException(nameof(connector));
            _connector = connector;
        }

        public void Execute(int jobID)
        {
            Execute(
                jobID,
                () => new ImportNewDropscanMailingsResult
                {
                    IDsOfImportedMailings = _connector.ImportNewMailings().ToArray()
                });
        }
    }
}