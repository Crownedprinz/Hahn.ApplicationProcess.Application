using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class AddFolderCommand : ICommand<AddFolder>
    {
        public void Execute(AddFolder parameters)
        {
            using (var context = new DatabaseContext())
            {
                context.Folders.Add(parameters.Folder);
                context.SaveChanges();
            }
        }
    }
}