using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public interface IQueryAll
    {
        IEnumerable<Asset> Execute();
        Asset ExecuteOne(int id);
    }
}
