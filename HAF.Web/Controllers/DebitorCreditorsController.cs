using System;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using HAF.Web.Resources;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;

namespace HAF.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "debitor-creditors")]
    public class DebitorCreditorsController : ResourceApiController<DebitorCreditor, DebitorCreditorResource>
    {
        private readonly ICommand<AddDebitorCreditor> _addDebitorCreditor;

        public DebitorCreditorsController(
            IQueryAll<DebitorCreditor> queryAll,
            IQuerySingle<DebitorCreditor> querySingle,
            ICommand<AddDebitorCreditor> addDebitorCreditor)
            : base(queryAll, querySingle)
        {
            if (addDebitorCreditor == null)
                throw new ArgumentNullException(nameof(addDebitorCreditor));
            _addDebitorCreditor = addDebitorCreditor;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Add([FromBody] AddOrUpdateDebitorCreditorResource resource)
        {
            var debitorCreditor = new DebitorCreditor { Name = resource.Name, CompanyID = resource.CompanyID };
            if (resource.RootFolderID.HasValue)
                debitorCreditor.RootFolderID = resource.RootFolderID.Value;
            else
                debitorCreditor.RootFolder = new Folder { Name = "/" };

            _addDebitorCreditor.Execute(new AddDebitorCreditor(debitorCreditor));

            return Created(debitorCreditor.ID.ToString(), ToResource(debitorCreditor));
        }

        public static DebitorCreditorResource ToResource(HttpRequestMessage request, DebitorCreditor entity) =>
            Mapper.Map<DebitorCreditor, DebitorCreditorResource>(entity);

        protected override object OrderByProperty(DebitorCreditor entity) => entity.Name.ToLowerInvariant();
        protected override DebitorCreditorResource ToResource(DebitorCreditor entity) => ToResource(Request, entity);
    }
}