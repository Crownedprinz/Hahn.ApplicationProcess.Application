using System.Linq;
using HAF.Domain.Entities;

namespace  HAF.DAL.Queries
{
    public class QueryFolders : QueryEntities<Folder>
    {
        private const string QueryCompanyAndDebitorOfFolder = @"
WITH recursiveFolders (ID, ParentFolderID, Name, Level)
AS
(
	Select f.ID, f.ParentFolderID, f.Name, 1 as Level From dbo.Folders f
	Union ALL
	Select rf.ID, f.ParentFolderID, rf.Name, 1 + rf.Level as Level From dbo.Folders f
	inner join recursiveFolders rf on f.ID = rf.ParentFolderID
), foldersWithRootTmp AS 
(
	select ParentFolderID as RootFolderID, ID as FolderID, Name, Level, RN from (
		SELECT *, ROW_NUMBER() OVER (PARTITION BY rf.ID ORDER BY rf.Level DESC) RN
		FROM recursiveFolders rf
	) s 
), foldersWithRoot AS
(
	select FolderID as RootFolderID, FolderID, Name, Level, RN from foldersWithRootTmp where RN = 1
	union all
	select * from foldersWithRootTmp where RN = 2
)
select c.ID as CompanyID, dc.ID as DebitorCreditorID from Companies c
inner join DebitorCreditors dc on c.ID = dc.CompanyID
inner join foldersWithRoot f on dc.RootFolderID = f.RootFolderID
and f.FolderID = @p0";

        public static CompanyAndDebitorCreditorOfFolder
            QueryCompanyAndDebitorCreditor(DatabaseContext context, int folderID) =>
            context.Database.SqlQuery<CompanyAndDebitorCreditorOfFolder>(QueryCompanyAndDebitorOfFolder, folderID)
                .SingleOrDefault();

        protected override IQueryable<Folder> CreateQuery(DatabaseContext context) =>
            context.Folders.Include("Files.AssignedFlags");
    }
}