using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using HAF.Domain;
using HAF.Domain.Connectors;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;
using HAF.Domain.Services;
using HAF.Web.Resources;
using Newtonsoft.Json.Converters;
using Swagger.Net.Examples;

namespace HAF.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "asset")]
    public class AssetController : ResourceApiController<Asset, Asset>
    {
        //private readonly IQueryAll<DocumentFlag> _queryDocumentFlags;
        //private readonly IQuery<QueryDocumentWithData, Document> _queryDocumentWithData;
        //private readonly ICommand<UpdateDocument> _updateDocument;
        private readonly IAssetService _assetService;
        public AssetController(
            IQueryAll<Asset> queryAll,
            IQuerySingle<Asset> querySingle,
            IAssetService assetService
            )
            : base(queryAll, querySingle)
        {
            _assetService = assetService;
        }



    

        [HttpPost]
        [Route("")]
        [SwaggerRequestExample(typeof(AddOrUpdateAssetResource), typeof(AssetExample), jsonConverter:typeof(StringEnumConverter))]
        public IHttpActionResult Add([FromBody] AddOrUpdateAssetResource resource)
        {
            var result = Mapper.Map<AddOrUpdateAssetResource, Asset>(resource);
            //validate object
            var validateResult = _assetService.validateAsset(result);
            if (!validateResult.flag)
            {
                return BadRequest(validateResult.errors);
            }
            //_addDebitorCreditor.Execute(new AddDebitorCreditor(debitorCreditor));

            return Created(result.ID.ToString(), ToResource(result));
        }

       
        [Route("countries")]
        public IHttpActionResult GetCountries()
        {
            var countries = CountryApi.GetCountries();
            if (countries == null)
                return NotFound();

            return Content(HttpStatusCode.OK, countries);
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerRequestExample(typeof(AddOrUpdateAssetResource), typeof(AssetExample), jsonConverter: typeof(StringEnumConverter))]
        public IHttpActionResult Update(int id, [FromBody] AddOrUpdateAssetResource config)
        {
            var result = Mapper.Map<AddOrUpdateAssetResource, Asset>(config);
            //validate object
            var validateResult = _assetService.validateAsset(result);
            if (!validateResult.flag)
            {
                return BadRequest(validateResult.errors);
            }

            var asset = _querySingle.Execute(id);
            if (asset == null)
                return NotFound();

            if (config.AssetName != null)
                asset.AssetName = config.AssetName;
            if (config.Department != null)
                asset.Department = config.Department;
            if (config.CountryOfDepartment != null)
                asset.CountryOfDepartment = config.CountryOfDepartment;
            if (config.EMailAdressOfDepartment != null)
                asset.EMailAdressOfDepartment = config.EMailAdressOfDepartment;
            if (config.PurchaseDate != null)
                asset.PurchaseDate = config.PurchaseDate;
            if (config.broken != asset.broken)
                asset.broken = config.broken;

            //_updateDocument.Execute(new UpdateDocument(document));

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var asset = _querySingle.Execute(id);
            if (asset == null)
                return NotFound();
            //_deleteDocument.Execute(new UpdateDocument(document));

            return Ok();
        }

        protected override object OrderByProperty(Asset entity) => entity.PurchaseDate;
      
    }
}