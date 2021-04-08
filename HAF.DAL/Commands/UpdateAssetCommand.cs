using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class UpdateAssetCommand : ICommand<UpdateAsset>
    {
        public void Execute(UpdateAsset parameters)
        {
            using (var context = new DatabaseContext())
            {
                
                var asset = parameters.Asset;
                 context.Asset.Add(asset);
                context.SaveChanges();
            }
        }
    }
}