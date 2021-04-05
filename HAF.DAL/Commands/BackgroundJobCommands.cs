using System;
using System.Linq;
using Newtonsoft.Json;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;

namespace  HAF.DAL.Commands
{
    public class BackgroundJobCommands : ICommand<AddBackgroundJob>,
                                         ICommand<StartBackgroundJobProcessing>,
                                         ICommand<FailBackgroundJob>,
                                         ICommand<SucceedBackgroundJob>
    {
        public void Execute(AddBackgroundJob parameters)
        {
            WithContext(c => c.BackgroundJobs.Add(parameters.BackgroundJob));
        }

        public void Execute(FailBackgroundJob parameters)
        {
            WithContext(c => { SetStatusAndResult(c, parameters.JobID, JobStatus.Failed, parameters.Error); });
        }

        public void Execute(StartBackgroundJobProcessing parameters)
        {
            WithContext(c => { SetStatus(c, parameters.JobID, JobStatus.Processing); });
        }

        public void Execute(SucceedBackgroundJob parameters)
        {
            WithContext(c => { SetStatusAndResult(c, parameters.JobID, JobStatus.Succeeded, parameters.Result); });
        }

        private static BackgroundJob SetStatus(DatabaseContext c, int jobId, JobStatus status)
        {
            var job = c.BackgroundJobs.SingleOrDefault(x => x.ID == jobId);
            if (job == null)
                throw new EntityNotFoundException<BackgroundJob>(x => x.ID, jobId);
            job.JobStatus = status;
            return job;
        }

        private static void SetStatusAndResult(DatabaseContext c, int jobId, JobStatus status, object result)
        {
            var job = SetStatus(c, jobId, status);
            if (result != null)
            {
                job.ResultType = result.GetType();
                job.ResultJson = JsonConvert.SerializeObject(result);
            }
        }

        private static void WithContext(Action<DatabaseContext> actionWithContext, bool needsSaving = true)
        {
            using (var context = new DatabaseContext())
            {
                actionWithContext(context);
                if (needsSaving)
                    context.SaveChanges();
            }
        }
    }
}