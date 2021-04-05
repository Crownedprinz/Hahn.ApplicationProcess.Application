using  HAF.Domain.Entities;

namespace  HAF.Domain.QueryParameters
{
    public class QueryDocumentWithData : IQueryParameters<Document>
    {
        public QueryDocumentWithData(int id, bool getContextData, bool getDocumentData)
        {
            GetContextData = getContextData;
            GetDocumentData = getDocumentData;
            ID = id;
        }

        public bool GetContextData { get; }
        public bool GetDocumentData { get; }
        public int ID { get; }
    }
}