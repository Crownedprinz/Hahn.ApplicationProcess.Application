using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryCompanies : QueryEntities<Company>, IQuery<CompanyOfFolder, Company>
    {
        public Company Execute(CompanyOfFolder parameters)
        {
            using (var context = new DatabaseContext())
            {
                var companyId = QueryFolders.QueryCompanyAndDebitorCreditor(context, parameters.FolderID)?.CompanyID;
                if (companyId == null)
                    return null;

                return Execute(companyId.Value);
            }
        }

        protected override IQueryable<Company> CreateQuery(DatabaseContext context)
        {
            return context.Companies.Include(x => x.AllowedDocumentFlags).Include("DebitorCreditors.RootFolder");
        }

        protected override Company PostProcess(Company entity)
        {
            foreach (var debitorCreditor in entity.DebitorCreditors)
            {
                debitorCreditor.RootFolder.Folders =
                    QueryDebitorCreditors.LoadFolderHierarchy(debitorCreditor.RootFolder.ID);
            }

            return entity;
        }
    }
}