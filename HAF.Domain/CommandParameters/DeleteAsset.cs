using HAF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.CommandParameters
{
    public class DeleteAsset
    {
        public DeleteAsset(Asset asset)
        {
            Asset = asset ?? throw new ArgumentNullException(nameof(asset));
        }

        public Asset Asset { get; }
    }
}
