using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public class DebitorCreditorOfFolder : IQueryParameters<Company>
    {
        public DebitorCreditorOfFolder(int folderId) => FolderID = folderId;
        public int FolderID { get; }
    }
}