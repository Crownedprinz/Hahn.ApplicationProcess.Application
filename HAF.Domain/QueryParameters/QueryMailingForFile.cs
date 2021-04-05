using  HAF.Domain.Entities;

namespace  HAF.Domain.QueryParameters
{
    public class QueryMailingForFile : IQueryParameters<DropscanMailing>
    {
        public QueryMailingForFile(int id) => ID = id;
        public int ID { get; }
    }
}