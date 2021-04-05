namespace HAF.Domain.CommandParameters
{
    public class StartBackgroundJobProcessing
    {
        public StartBackgroundJobProcessing(int jobID) => JobID = jobID;
        public int JobID { get; }
    }
}