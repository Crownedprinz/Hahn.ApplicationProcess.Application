using System;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace HAF.Web.BackgroundJobs
{
    public abstract class BackgroundJobHandler<TResult>
    {
        private readonly ICommand<FailBackgroundJob> _failJob;
        private readonly ICommand<StartBackgroundJobProcessing> _startJobProcessing;
        private readonly ICommand<SucceedBackgroundJob> _succeedJob;

        protected BackgroundJobHandler(
            ICommand<StartBackgroundJobProcessing> startJobProcessing,
            ICommand<SucceedBackgroundJob> succeedJob,
            ICommand<FailBackgroundJob> failJob)
        {
            if (failJob == null)
                throw new ArgumentNullException(nameof(failJob));
            if (startJobProcessing == null)
                throw new ArgumentNullException(nameof(startJobProcessing));
            if (succeedJob == null)
                throw new ArgumentNullException(nameof(succeedJob));
            _failJob = failJob;
            _startJobProcessing = startJobProcessing;
            _succeedJob = succeedJob;
        }

        protected void Execute(int jobID, Func<TResult> action)
        {
            _startJobProcessing.Execute(new StartBackgroundJobProcessing(jobID));
            try
            {
                var result = action();
                _succeedJob.Execute(new SucceedBackgroundJob(jobID, result));
            }
            catch (Exception e)
            {
                _failJob.Execute(new FailBackgroundJob(jobID, e));
                throw;
            }
        }
    }
}