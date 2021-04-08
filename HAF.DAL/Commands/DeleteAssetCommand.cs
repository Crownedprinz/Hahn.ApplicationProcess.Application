using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class DeleteAssetCommand : ICommand<DeleteAsset>
    {
        public void Execute(DeleteAsset parameters)
        {
            using (var context = new DatabaseContext())
            {
                
                var asset = parameters.Asset;
                context.Asset.Remove(asset);
                context.SaveChanges();
            }
        }
    }
}