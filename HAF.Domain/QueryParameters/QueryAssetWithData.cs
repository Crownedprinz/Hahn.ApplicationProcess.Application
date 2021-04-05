using HAF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.QueryParameters
{
 
    public class QueryAssetWithData : IQueryParameters<Asset>
    {
        public QueryAssetWithData(int id, bool getContextData, bool getDocumentData)
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
