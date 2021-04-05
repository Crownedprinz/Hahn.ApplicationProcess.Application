using HAF.Domain.Entities;

namespace  HAF.Domain.QueryParameters
{
    public class QueryMailingWithData : IQueryParameters<DropscanMailing>
    {
        public QueryMailingWithData(int id, bool getEnvelopeData, bool getFileData)
        {
            GetEnvelopeData = getEnvelopeData;
            GetFileData = getFileData;
            ID = id;
        }

        public bool GetEnvelopeData { get; }
        public bool GetFileData { get; }
        public int ID { get; }
    }
}