using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HAF.Web.BackgroundJobs;
using HAF.Web.Resources;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;
using BackgroundJobProcessor = Hangfire.BackgroundJob;

namespace HAF.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "dropscan-mailings")]
    public class DropscanMailingsController : ResourceApiController<DropscanMailing, DropscanMailingResource>
    {
        private readonly ICommand<AddBackgroundJob> _addBackgroundJob;
        private readonly ICommand<AddDropscanMailingAsDocument> _addDropscanMailingAsDocument;
        private readonly ICommand<AddDropscanMailingPagesAsDocument> _addDropscanMailingPagesAsDocument;
        private readonly ICommand<DiscardPages> _discardPages;
        private readonly IQuery<DropscanMailingsByStatus, IEnumerable<DropscanMailing>> _queryByStatus;
        private readonly IQuery<DropscanMailingPageByPageNumber, DropscanMailingPage> _queryDropscanMailingPage;
        private readonly IQuery<PagesOfDropscanMailing, DropscanMailingPages> _queryDropscanMailingPages;
        private readonly IQuery<QueryMailingForFile, DropscanMailing> _queryMailingForFile;
        private readonly IQuery<QueryMailingWithData, DropscanMailing> _queryMailingWithData;
        private readonly IQuery<DropscanMailingWithPages, DropscanMailing> _queryMailingWithPages;
        private readonly IQuerySingle<DocumentFlag> _querySingleDocumentFlag;
        private readonly IQuerySingle<DropscanMailing> _querySingleDropscanMailing;
        private readonly ICommand<UndiscardPages> _undiscardPages;

        public DropscanMailingsController(
            IQueryAll<DropscanMailing> queryAllDropscanMailings,
            IQuerySingle<DropscanMailing> querySingleDropscanMailing,
            ICommand<AddDropscanMailingAsDocument> addDropscanMailingAsDocument,
            IQuerySingle<DocumentFlag> querySingleDocumentFlag,
            IQuery<PagesOfDropscanMailing, DropscanMailingPages> queryDropscanMailingPages,
            IQuery<DropscanMailingPageByPageNumber, DropscanMailingPage> queryDropscanMailingPage,
            ICommand<AddBackgroundJob> addBackgroundJob,
            ICommand<DiscardPages> discardPages,
            ICommand<AddDropscanMailingPagesAsDocument> addDropscanMailingPagesAsDocument,
            IQuery<DropscanMailingsByStatus, IEnumerable<DropscanMailing>> queryByStatus,
            ICommand<UndiscardPages> undiscardPages,
            IQuery<DropscanMailingWithPages, DropscanMailing> queryMailingWithPages,
            IQuery<QueryMailingWithData, DropscanMailing> queryMailingWithData,
            IQuery<QueryMailingForFile, DropscanMailing> queryMailingForFile)
            : base(queryAllDropscanMailings, querySingleDropscanMailing)
        {
            if (querySingleDropscanMailing == null)
                throw new ArgumentNullException(nameof(querySingleDropscanMailing));
            if (addDropscanMailingAsDocument == null)
                throw new ArgumentNullException(nameof(addDropscanMailingAsDocument));
            if (querySingleDocumentFlag == null)
                throw new ArgumentNullException(nameof(querySingleDocumentFlag));
            if (queryDropscanMailingPages == null)
                throw new ArgumentNullException(nameof(queryDropscanMailingPages));
            if (addBackgroundJob == null)
                throw new ArgumentNullException(nameof(addBackgroundJob));
            if (discardPages == null)
                throw new ArgumentNullException(nameof(discardPages));
            if (addDropscanMailingPagesAsDocument == null)
                throw new ArgumentNullException(nameof(addDropscanMailingPagesAsDocument));
            if (queryByStatus == null)
                throw new ArgumentNullException(nameof(queryByStatus));
            if (undiscardPages == null)
                throw new ArgumentNullException(nameof(undiscardPages));
            if (queryMailingWithPages == null)
                throw new ArgumentNullException(nameof(queryMailingWithPages));
            if (queryMailingWithData == null)
                throw new ArgumentNullException(nameof(queryMailingWithData));
            _querySingleDropscanMailing = querySingleDropscanMailing;
            _addDropscanMailingAsDocument = addDropscanMailingAsDocument;
            _querySingleDocumentFlag = querySingleDocumentFlag;
            _queryDropscanMailingPages = queryDropscanMailingPages;
            _queryDropscanMailingPage = queryDropscanMailingPage;
            _addBackgroundJob = addBackgroundJob;
            _discardPages = discardPages;
            _addDropscanMailingPagesAsDocument = addDropscanMailingPagesAsDocument;
            _queryByStatus = queryByStatus;
            _undiscardPages = undiscardPages;
            _queryMailingWithPages = queryMailingWithPages;
            _queryMailingWithData = queryMailingWithData;
            _queryMailingForFile = queryMailingForFile;
        }

        [HttpPost]
        [Route("{id}/documents")]
        public IHttpActionResult AddAsDocument(int id, [FromBody] AddOrUpdateDocumentResource config)
        {
            var dropscanMailing = _queryMailingForFile.Execute(new QueryMailingForFile(id));
            if (dropscanMailing == null)
                return NotFound();

            var assignedFlags = new List<DocumentFlag>();
            if (config.AssignedFlags != null)
            {
                assignedFlags = config.AssignedFlags.Distinct().Select(_querySingleDocumentFlag.Execute).ToList();
                if (assignedFlags.Any(x => x == null))
                    return NotFound();
            }

            var document = new Document
            {
                ContextData = dropscanMailing.EnvelopeData,
                ContextDataContentType = "image/jpg",
                ContextFileExtension = "jpg",
                Direction = config.Direction,
                DocumentData = new DocumentData(dropscanMailing.FileData.Data),
                DocumentDataContentType = "application/pdf",
                DocumentFileExtension = "pdf",
                Name = config.Name,
                FolderID = config.FolderID,
                AssignedFlags = assignedFlags,
                DebitorCreditorID = config.DebitorCreditorID
            };
            if (config.Date != null)
                document.Date = config.Date.Value.Date;

            if (dropscanMailing.DiscardedPages.Any() || dropscanMailing.MappedPages.Any())
            {
                var pageNumbers = GetUnmappedPageNumbers(dropscanMailing);
                document.DocumentData = new DocumentData(
                    PdfHelpers.GetPdfWithPages(dropscanMailing.FileData.Data, pageNumbers));
                _addDropscanMailingPagesAsDocument.Execute(new AddDropscanMailingPagesAsDocument(id, document, pageNumbers));
            }
            else
                _addDropscanMailingAsDocument.Execute(new AddDropscanMailingAsDocument(id, document));

            return Created(UrlHelperEx.GetLink<DocumentsController>(Request, x => x.Get(document.ID)), ToResource(document));
        }

        [HttpPost]
        [Route("{id}/pages/documents")]
        public IHttpActionResult AddPagesAsDocument(int id, [FromBody] MulitplePagesResource config)
        {
            var dropscanMailing = _queryMailingWithData.Execute(new QueryMailingWithData(id, true, true));
            if (dropscanMailing == null)
                return NotFound();

            var document = new Document
            {
                ContextData = new DocumentData(dropscanMailing.EnvelopeData.Data),
                ContextDataContentType = "image/jpg",
                ContextFileExtension = "jpg",
                DocumentData =
                    new DocumentData(PdfHelpers.GetPdfWithPages(dropscanMailing.FileData.Data, config.PageNumbers)),
                DocumentDataContentType = "application/pdf",
                DocumentFileExtension = "pdf"
            };

            _addDropscanMailingPagesAsDocument.Execute(
                new AddDropscanMailingPagesAsDocument(id, document, config.PageNumbers));

            return Created(UrlHelperEx.GetLink<DocumentsController>(Request, x => x.Get(document.ID)), ToResource(document));
        }

        [HttpPost]
        [Route("{id}/pages/discarded")]
        public IHttpActionResult DiscardPages(int id, [FromBody] MulitplePagesResource config)
        {
            var dropscanMailing = _querySingleDropscanMailing.Execute(id);
            if (dropscanMailing == null)
                return NotFound();

            _discardPages.Execute(new DiscardPages(id, config.PageNumbers));

            return Ok();
        }

        [Route("added")]
        public IEnumerable<DropscanMailingResource> GetAdded() =>
            _queryByStatus.Execute(new DropscanMailingsByStatus(DropscanMailingMappingStatus.AddedAsDocument))
                .Select(ToResource);

        [Route("discarded")]
        public IEnumerable<DropscanMailingResource> GetDiscarded() =>
            _queryByStatus.Execute(new DropscanMailingsByStatus(DropscanMailingMappingStatus.Discarded)).Select(ToResource);

        [HttpGet]
        [Route("{id}/pages/discarded")]
        public IHttpActionResult GetDiscardedPages(int id)
        {
            var dropscanMailing = _queryMailingWithPages.Execute(new DropscanMailingWithPages(id));
            if (dropscanMailing == null)
                return NotFound();

            return Content(
                HttpStatusCode.OK,
                dropscanMailing.DiscardedPages.Select(
                    x => new DiscardedDropscanMailingPageResource
                    {
                        MailingID = x.MailingID,
                        PageNumber = x.PageNumber,
                        PageLink = UrlHelperEx.GetLink<DropscanMailingsController>(
                            Request,
                            y => y.GetPage(x.MailingID, x.PageNumber))
                    }));
        }

        [Route("{id}/envelope")]
        public HttpResponseMessage GetEnvelope(int id)
        {
            return CreateFileResponseFromDocument(id, true, false, x => x.EnvelopeData.Data, "image/jpg", "jpg");
        }

        [Route("{id}/pdf")]
        public HttpResponseMessage GetFile(int id)
        {
            var mailing = _queryMailingForFile.Execute(new QueryMailingForFile(id));
            if (mailing == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var contentType = "application/pdf";
            var fileName = $"{mailing.Uuid}.pdf";
            if (mailing.DiscardedPages.Any() || mailing.MappedPages.Any())
            {
                var pageNumbers = GetUnmappedPageNumbers(mailing);
                var data = PdfHelpers.GetPdfWithPages(mailing.FileData.Data, pageNumbers);
                return FileResponse(contentType, fileName, data);
            }

            return FileResponse(contentType, fileName, mailing.FileData.Data);
        }

        [Route("finished")]
        public IEnumerable<DropscanMailingResource> GetFinished() =>
            _queryByStatus.Execute(
                    new DropscanMailingsByStatus(
                        DropscanMailingMappingStatus.Discarded,
                        DropscanMailingMappingStatus.AddedAsDocument,
                        DropscanMailingMappingStatus.Splitted))
                .Select(ToResource);

        [Route("open")]
        public IEnumerable<DropscanMailingResource> GetOpen() =>
            _queryByStatus.Execute(
                    new DropscanMailingsByStatus(
                        DropscanMailingMappingStatus.Imported,
                        DropscanMailingMappingStatus.PartiallySplitted))
                .Select(ToResource);

        [Route("{id}/pdf/original")]
        public HttpResponseMessage GetOriginalFile(int id)
        {
            return CreateFileResponseFromDocument(id, false, true, x => x.FileData.Data, "application/pdf", "pdf");
        }

        [Route("{mailingID}/pages/{pageNumber}")]
        public HttpResponseMessage GetPage(int mailingID, int pageNumber)
        {
            var page = _queryDropscanMailingPage.Execute(new DropscanMailingPageByPageNumber(mailingID, pageNumber));
            if (page == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            return FileResponse(page.PageContentType, $"{mailingID}-page-{pageNumber}.png", page.PageData.Data);
        }

        [Route("{id}/pages")]
        public IHttpActionResult GetPages(int id)
        {
            var dropscanMailingPages = _queryDropscanMailingPages.Execute(new PagesOfDropscanMailing(id));
            if (dropscanMailingPages == null || !dropscanMailingPages.DiscardedPages.Any() &&
                !dropscanMailingPages.MappedPages.Any() && !dropscanMailingPages.UnmappedPages.Any())
                return NotFound();

            return Content(HttpStatusCode.OK, DropscanMailingPagesResourceFactory.Create(dropscanMailingPages, Request));
        }

        [Route("splitted")]
        public IEnumerable<DropscanMailingResource> GetSplitted() =>
            _queryByStatus.Execute(
                    new DropscanMailingsByStatus(
                        DropscanMailingMappingStatus.Splitted,
                        DropscanMailingMappingStatus.PartiallySplitted))
                .Select(ToResource);

        [HttpPost]
        [Route("import")]
        public IHttpActionResult StartImport()
        {
            var job = new BackgroundJob();
            _addBackgroundJob.Execute(new AddBackgroundJob(job));

            BackgroundJobProcessor.Enqueue<ImportNewDropscanMailingsJob>(x => x.Execute(job.ID));
            return Created(
                UrlHelperEx.GetLink<BackgroundJobsController>(Request, x => x.Get(job.ID)),
                BackgroundJobsController.ToResource(Request, job));
        }

        [HttpPost]
        [Route("{id}/pages")]
        public IHttpActionResult StartPagesGeneration(int id, bool recreateIfExist = false)
        {
            var dropscanMailing = _querySingleDropscanMailing.Execute(id);
            if (dropscanMailing == null)
                return NotFound();

            var job = new BackgroundJob();
            _addBackgroundJob.Execute(new AddBackgroundJob(job));

            BackgroundJobProcessor.Enqueue<PreparePagesFromDropscanMailingJob>(x => x.Execute(id, job.ID, recreateIfExist));
            return Created(
                UrlHelperEx.GetLink<BackgroundJobsController>(Request, x => x.Get(job.ID)),
                BackgroundJobsController.ToResource(Request, job));
        }

        [HttpPost]
        [Route("{id}/pages/undiscarded")] // TODO: Improve route/verb
        public IHttpActionResult UndiscardPages(int id, [FromBody] MulitplePagesResource config)
        {
            var dropscanMailing = _querySingleDropscanMailing.Execute(id);
            if (dropscanMailing == null)
                return NotFound();

            _undiscardPages.Execute(new UndiscardPages(id, config.PageNumbers));

            return Ok();
        }

        protected override object OrderByProperty(DropscanMailing entity) => entity.ReceivedAt;

        protected override DropscanMailingResource ToResource(DropscanMailing entity)
        {
            var result = base.ToResource(entity);
            result.Envelope = UrlHelperEx.GetLink<DropscanMailingsController>(Request, x => x.GetEnvelope(entity.ID));
            result.Pdf = UrlHelperEx.GetLink<DropscanMailingsController>(Request, x => x.GetFile(entity.ID));
            return result;
        }

        private HttpResponseMessage CreateFileResponseFromDocument(
            int id,
            bool getEnvelopeData,
            bool getFileData,
            Func<DropscanMailing, byte[]> data,
            string contentType,
            string fileExtension)
        {
            var mailing = _queryMailingWithData.Execute(new QueryMailingWithData(id, getEnvelopeData, getFileData));
            if (mailing == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            return FileResponse(contentType, $"{mailing.Uuid}.{fileExtension}", data(mailing));
        }

        private static List<int> GetUnmappedPageNumbers(DropscanMailing dropscanMailing)
        {
            var pageNumbers = dropscanMailing.Pages.Select(x => x.PageNumber)
                .Except(dropscanMailing.DiscardedPages.Select(x => x.PageNumber))
                .Except(dropscanMailing.MappedPages.Select(x => x.PageNumber))
                .ToList();
            return pageNumbers;
        }

        private DocumentResource ToResource(Document document) => DocumentsController.ToResource(Request, document);
    }
}