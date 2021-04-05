namespace HAF.Web.BackgroundJobs
{
    public class PreparePagesFromDropscanMailingResult
    {
        public int MailingID { get; set; }
        public int[] PageNumbers { get; set; }
    }
}