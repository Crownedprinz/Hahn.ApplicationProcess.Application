
using HAF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.Services
{
    public interface IAssetService
    {
        (bool flag, string errors) validateAsset(Asset dto);
    }
}
