using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AutoMapper;
using HAF.Domain;

namespace HAF.Web.Controllers
{
    public abstract class ResourceApiController<TEntity, TResource> : ApiController
    {
        protected readonly IQueryAll<TEntity> _queryAll;
        protected readonly IQuerySingle<TEntity> _querySingle;

        protected ResourceApiController(IQueryAll<TEntity> queryAll, IQuerySingle<TEntity> querySingle)
        {
            if (queryAll == null)
                throw new ArgumentNullException(nameof(queryAll));
            if (querySingle == null)
                throw new ArgumentNullException(nameof(querySingle));
            _queryAll = queryAll;
            _querySingle = querySingle;
        }

        [Route("")]
        public IEnumerable<TResource> Get() => _queryAll.Execute().OrderBy(OrderByProperty).Select(ToResource);

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var entity = _querySingle.Execute(id);
            if (entity == null)
                return NotFound();
            return Ok(ToResource(entity));
        }

        protected HttpResponseMessage FileResponse(string contentType, string fileName, byte[] content)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(content) };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline") { FileName = fileName };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return result;
        }

        protected abstract object OrderByProperty(TEntity entity);
        protected virtual TResource ToResource(TEntity entity) => Mapper.Map<TEntity, TResource>(entity);
    }
}