using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryDebitorCreditors : QueryEntities<DebitorCreditor>, IQuery<DebitorCreditorOfFolder, DebitorCreditor>
    {
        private const string Query = @"
 WITH recursiveFolders (ID, ParentID, Name)
 AS
 (
    Select f.ID, f.ParentFolderID, f.Name From dbo.Folders f where f.ParentFolderID = @p0
    Union ALL
    Select f.ID, f.ParentFolderID, f.Name From dbo.Folders f
    inner join recursiveFolders rf on f.ParentFolderID = rf.ID
 )

 Select ID, ParentID, Name from recursiveFolders";

        public DebitorCreditor Execute(DebitorCreditorOfFolder parameters)
        {
            using (var context = new DatabaseContext())
            {
                var debitorCreditorID = QueryFolders.QueryCompanyAndDebitorCreditor(context, parameters.FolderID)
                    ?.DebitorCreditorID;
                if (debitorCreditorID == null)
                    return null;

                return Execute(debitorCreditorID.Value);
            }
        }

        public static List<Folder> LoadFolderHierarchy(int rootID)
        {
            using (var context = new DatabaseContext())
            {
                var dbFolders = context.Database.SqlQuery<FolderDbObject>(Query, rootID).ToList();
                return new List<Folder>(GetFolders(dbFolders, rootID));
            }
        }

        protected override IQueryable<DebitorCreditor> CreateQuery(DatabaseContext context)
        {
            return context.DebitorCreditors.Include(x => x.RootFolder);
        }

        protected override DebitorCreditor PostProcess(DebitorCreditor entity)
        {
            entity.RootFolder.Folders = LoadFolderHierarchy(entity.RootFolder.ID);

            return entity;
        }

        private static List<Folder> GetFolders(ICollection<FolderDbObject> dbFolders, int rootID)
        {
            return dbFolders.Where(x => x.ParentID == rootID)
                .Select(x => new Folder { ID = x.ID, Name = x.Name, Folders = GetFolders(dbFolders, x.ID) })
                .ToList();
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class FolderDbObject
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ParentID { get; set; }
        }
    }
}