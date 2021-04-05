namespace HAF.Domain.CommandParameters
{
    public class SucceedBackgroundJob
    {
        public SucceedBackgroundJob(int jobID, object result)
        {
            Result = result;
            JobID = jobID;
        }

        public int JobID { get; }
        public object Result { get; set; }
    }
}