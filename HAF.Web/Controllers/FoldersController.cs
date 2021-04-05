using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using HAF.Web.Resources;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace HAF.Web.Controllers
{
    [RoutePrefix(Globals.ApiRoutesPrefix + "folders")]
    public class FoldersController : ResourceApiController<Folder, FolderResource>
    {
        private readonly ICommand<AddFolder> _addFolder;
        private readonly IQuery<CompanyOfFolder, Company> _queryCompanyOfFolder;
        private readonly IQuery<DebitorCreditorOfFolder, DebitorCreditor> _queryDebitorCreditorOfFolder;

        public FoldersController(
            IQueryAll<Folder> queryAll,
            IQuerySingle<Folder> querySingle,
            IQuery<CompanyOfFolder, Company> queryCompanyOfFolder,
            IQuery<DebitorCreditorOfFolder, DebitorCreditor> queryDebitorCreditorOfFolder,
            ICommand<AddFolder> addFolder)
            : base(queryAll, querySingle)
        {
            if (queryCompanyOfFolder == null)
                throw new ArgumentNullException(nameof(queryCompanyOfFolder));
            if (queryDebitorCreditorOfFolder == null)
                throw new ArgumentNullException(nameof(queryDebitorCreditorOfFolder));
            if (addFolder == null)
                throw new ArgumentNullException(nameof(addFolder));
            _queryCompanyOfFolder = queryCompanyOfFolder;
            _queryDebitorCreditorOfFolder = queryDebitorCreditorOfFolder;
            _addFolder = addFolder;
        }

        [HttpPost]
        [Route("{parentFolderId}/{newFolderName}")]
        public IHttpActionResult Add(int parentFolderId, string newFolderName)
        {
            var parentFolder = _querySingle.Execute(parentFolderId);
            if (parentFolder == null)
                return NotFound();
            var folder = new Folder { Name = newFolderName, ParentFolderID = parentFolderId };
            _addFolder.Execute(new AddFolder(folder));
            return Content(HttpStatusCode.OK, ToResource(folder));
        }

        [Route("{id}/company")]
        public IHttpActionResult GetCorrespondingCompany(int id)
        {
            var company = _queryCompanyOfFolder.Execute(new CompanyOfFolder(id));
            if (company == null)
                return NotFound();

            return Content(HttpStatusCode.OK, CompaniesController.ToResource(Request, company));
        }

        [Route("{id}/debitor-creditor")]
        public IHttpActionResult GetCorrespondingDebitorCreditor(int id)
        {
            var debitorCreditor = _queryDebitorCreditorOfFolder.Execute(new DebitorCreditorOfFolder(id));
            if (debitorCreditor == null)
                return NotFound();

            return Content(HttpStatusCode.OK, DebitorCreditorsController.ToResource(Request, debitorCreditor));
        }

        [Route("{id}/documents")]
        public IHttpActionResult GetDocuments(int id)
        {
            var folder = _querySingle.Execute(id);
            if (folder == null)
                return NotFound();

            return Content(HttpStatusCode.OK, folder.Files.Select(x => DocumentsController.ToResource(Request, x)));
        }

        protected override object OrderByProperty(Folder entity) => entity.Name;
    }
}