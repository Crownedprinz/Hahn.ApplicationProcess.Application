using System;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using HAF.Web.Resources;
using HAF.Domain;
using HAF.Domain.Entities;

namespace HAF.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "jobs")]
    public class BackgroundJobsController : ResourceApiController<BackgroundJob, BackgroundJobResource>
    {
        private readonly IQuerySingle<BackgroundJob> _querySingleBackgroundJob;

        public BackgroundJobsController(
            IQueryAll<BackgroundJob> queryAll,
            IQuerySingle<BackgroundJob> querySingleBackgroundJob)
            : base(queryAll, querySingleBackgroundJob)
        {
            if (querySingleBackgroundJob == null)
                throw new ArgumentNullException(nameof(querySingleBackgroundJob));
            _querySingleBackgroundJob = querySingleBackgroundJob;
        }

        public static BackgroundJobResource ToResource(HttpRequestMessage request, BackgroundJob entity)
        {
            var result = Mapper.Map<BackgroundJob, BackgroundJobResource>(entity);
            result.UpdatedStatusLink = UrlHelperEx.GetLink<BackgroundJobsController>(request, x => x.Get(entity.ID));
            return result;
        }

        protected override object OrderByProperty(BackgroundJob entity) => entity.ID;
        protected override BackgroundJobResource ToResource(BackgroundJob entity) => ToResource(Request, entity);
    }
}