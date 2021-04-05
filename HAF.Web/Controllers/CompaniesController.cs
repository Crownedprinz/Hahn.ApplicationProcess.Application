using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using HAF.Web.Resources;
using HAF.Domain;
using HAF.Domain.Entities;

namespace HAF.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "companies")]
    public class CompaniesController : ResourceApiController<Company, CompanyResource>
    {
        public CompaniesController(IQueryAll<Company> queryAll, IQuerySingle<Company> querySingle)
            : base(queryAll, querySingle)
        {
        }

        public static CompanyResource ToResource(HttpRequestMessage request, Company entity) =>
            Mapper.Map<Company, CompanyResource>(entity);

        protected override object OrderByProperty(Company entity) => entity.Name.ToLowerInvariant();
        protected override CompanyResource ToResource(Company entity) => ToResource(Request, entity);
    }
}