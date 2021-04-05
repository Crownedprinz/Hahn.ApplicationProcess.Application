using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public class CompanyOfFolder : IQueryParameters<Company>
    {
        public CompanyOfFolder(int folderId) => FolderID = folderId;
        public int FolderID { get; }
    }
}