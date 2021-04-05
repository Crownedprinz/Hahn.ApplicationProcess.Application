using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class AddCompaniesCommand : ICommand<AddCompanies>
    {
        public void Execute(AddCompanies parameters)
        {
            using (var context = new DatabaseContext())
            {
                context.Companies.AddRange(parameters.CompaniesToAdd);
                context.SaveChanges();
            }
        }
    }
}