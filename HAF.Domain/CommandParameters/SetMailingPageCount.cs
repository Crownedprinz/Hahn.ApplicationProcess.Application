namespace HAF.Domain.CommandParameters
{
    public class SetMailingPageCount
    {
        public SetMailingPageCount(int id, int pageCount)
        {
            ID = id;
            PageCount = pageCount;
        }

        public int ID { get; }
        public int PageCount { get; }
    }
}