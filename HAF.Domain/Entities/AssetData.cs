using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.Entities
{
    public class AssetData : Entity
    {
        public AssetData()
        {
        }

        public AssetData(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public byte[] Data { get; set; }
    }
}
