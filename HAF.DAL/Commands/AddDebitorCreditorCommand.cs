using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class AddDebitorCreditorCommand : ICommand<AddDebitorCreditor>
    {
        public void Execute(AddDebitorCreditor parameters)
        {
            using (var context = new DatabaseContext())
            {
                context.DebitorCreditors.Add(parameters.DebitorCreditor);
                context.SaveChanges();
            }
        }
    }
}