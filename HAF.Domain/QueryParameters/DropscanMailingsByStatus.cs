using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public class DropscanMailingsByStatus : IQueryParameters<IEnumerable<DropscanMailing>>
    {
        public DropscanMailingsByStatus(
            DropscanMailingMappingStatus mappingStatus,
            params DropscanMailingMappingStatus[] moreStatuses)
        {
            if (moreStatuses == null)
                throw new ArgumentNullException(nameof(moreStatuses));
            var statuses = new List<DropscanMailingMappingStatus> { mappingStatus };
            statuses.AddRange(moreStatuses);
            DropscanMailingStatuses = statuses;
        }

        public IEnumerable<DropscanMailingMappingStatus> DropscanMailingStatuses { get; }
    }
}