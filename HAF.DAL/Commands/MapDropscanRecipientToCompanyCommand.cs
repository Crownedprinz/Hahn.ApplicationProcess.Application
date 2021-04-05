using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;

namespace  HAF.DAL.Commands
{
    public class MapDropscanRecipientToCompanyCommand : ICommand<MapDropscanRecipientToCompany>
    {
        public void Execute(MapDropscanRecipientToCompany parameters)
        {
            using (var context = new DatabaseContext())
            {
                var recipient = context.DropscanRecipients.SingleOrDefault(x => x.Name == parameters.RecipientName);
                if (recipient == null)
                    throw new EntityNotFoundException<DropscanRecipient>(x => x.Name, parameters.RecipientName);

                var company = context.Companies.SingleOrDefault(x => x.Name == parameters.CompanyName);
                if (company == null)
                    throw new EntityNotFoundException<Company>(x => x.Name, parameters.CompanyName);

                recipient.CorrespondingCompany = company;
                context.SaveChanges();
            }
        }
    }
}