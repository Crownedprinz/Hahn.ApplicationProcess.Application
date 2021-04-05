namespace HAF.Web.Resources
{
    public class FolderResource : NamedEntityResource
    {
        public DocumentResource[] Files { get; set; }
        public FolderResource[] Folders { get; set; }
    }
}