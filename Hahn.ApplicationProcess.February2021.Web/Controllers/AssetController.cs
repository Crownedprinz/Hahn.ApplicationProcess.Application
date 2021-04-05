using AutoMapper;
using HAF.Domain;
using Hahn.ApplicationProcess.February2021.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Hahn.ApplicationProcess.February2021.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "asset")]
    public class AssetController : ResourceApiController<Asset, AssetResource>
    {
        public AssetController(
            IQueryAll<Asset> queryAll,
            IQuerySingle<Asset> querySingle) : base(queryAll, querySingle)
        {
           
        }

        //public static AssetResource ToResource(HttpRequestMessage request, Asset entity)
        //{
        //    var result = Mapper.Map<Asset, AssetResource>(entity);
        //    //result.Context = UrlHelperEx.GetLink<DocumentsController>(request, x => x.GetContext(entity.ID));
        //    //result.Document = UrlHelperEx.GetLink<DocumentsController>(request, x => x.GetFile(entity.ID));
        //    //result.CompanyID = entity.DebitorCreditor?.CompanyID;
        //    return result;
        //}

        //protected override object OrderByProperty(Asset entity) => entity.PurchaseDate;
        //protected override AssetResource ToResource(Asset entity) => ToResource(Request, entity);
        protected override object OrderByProperty(Asset entity) => entity.PurchaseDate;
    }
}
