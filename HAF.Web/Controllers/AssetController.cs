using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Connectors;
using HAF.Domain.Entities;
using HAF.Web.Resources;
using HAF.Domain.QueryParameters;
using HAF.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace HAF.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssetController : ControllerBase
    {
        protected readonly IQueryAll _queryAll;
        private readonly ICommand _command;
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;
        public AssetController(
            IQueryAll queryAll,
            IAssetService assetService,
            ICommand command,
            IMapper mapper)
        {
            if (queryAll == null)
                throw new ArgumentNullException(nameof(queryAll));
            _queryAll = queryAll;
            _assetService = assetService;
            _command = command;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        /// <summary>
        /// Retrieves a specific asset by  id
        /// </summary>
        /// <param name="id" example="1">The product id</param>
        public IActionResult Get(int id)
        {
            var entity = _queryAll.ExecuteOne(id);
            if (entity == null)
                return NotFound();
            return Ok(ToResource(entity));
        }

        
        [HttpGet]

        public IEnumerable<Asset> Get() => _queryAll.Execute().Select(ToResource);



        [HttpPost]
        public IActionResult Add([FromBody] AddOrUpdateAssetResource resource)
        {
            var result = new Asset
            {
                AssetName = resource.AssetName,
                broken = resource.broken,
                CountryOfDepartment = resource.CountryOfDepartment,
                Department = resource.Department,
                EMailAdressOfDepartment = resource.EMailAdressOfDepartment,
                PurchaseDate = resource.PurchaseDate,

            };
            //validate object
            var validateResult = _assetService.validateAsset(result);
            if (!validateResult.flag)
            {
                return BadRequest(validateResult.errors);
            }
           _command.ExecuteAdd(result);

            return Created(result.ID.ToString(), ToResource(result));
        }


        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var countries = CountryApi.GetCountries();
            if (countries == null)
                return NotFound();

            return Ok(countries);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AddOrUpdateAssetResource config)
        {
            var result = _mapper.Map<Asset>(config);
            //validate object
            var validateResult = _assetService.validateAsset(result);
            if (!validateResult.flag)
            {
                return BadRequest(validateResult.errors);
            }

            var asset = _queryAll.ExecuteOne(id);
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

            _command.ExecuteUpdate(asset);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var asset = _queryAll.ExecuteOne(id);
            if (asset == null)
                return NotFound();
            _command.ExecuteDelete(asset);

            return Ok();
        }


        protected virtual Asset ToResource(Entity entity) => _mapper.Map<Asset>(entity);
    }
}

