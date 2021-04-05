using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class AddRecipientsCommand : ICommand<AddRecipients>
    {
        public void Execute(AddRecipients parameters)
        {
            using (var context = new DatabaseContext())
            {
                context.DropscanRecipients.AddRange(parameters.RecipientsToAdd);
                context.SaveChanges();
            }
        }
    }
}