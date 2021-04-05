using System;

namespace HAF.Domain.CommandParameters
{
    public class FailBackgroundJob
    {
        public FailBackgroundJob(int jobID, Exception error)
        {
            JobID = jobID;
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public Exception Error { get; }
        public int JobID { get; }
    }
}