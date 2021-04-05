using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class AddMailingsCommand : ICommand<AddMailings>
    {
        public void Execute(AddMailings parameters)
        {
            using (var context = new DatabaseContext())
            {
                context.DropscanMailings.AddRange(parameters.MailingsToAdd);
                context.SaveChanges();
            }
        }
    }
}