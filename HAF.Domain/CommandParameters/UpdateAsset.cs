using System;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class UpdateAsset
    {
        public UpdateAsset(Asset asset)
        {
            Asset = asset ?? throw new ArgumentNullException(nameof(asset));
        }

        public Asset Asset { get; }
    }
}