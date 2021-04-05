using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace  HAF.DAL.Commands
{
    public class UpdateDocumentCommand : ICommand<UpdateDocument>
    {
        public void Execute(UpdateDocument parameters)
        {
            using (var context = new DatabaseContext())
            {
                var allFlags = context.DocumentFlags.ToList();
                var document = parameters.Document;
                var newFlags = document.AssignedFlags;
                document.AssignedFlags = null;
                context.Documents.AddOrUpdate(document);
                context.SaveChanges();
                var dbDocument = context.Documents.Include(x => x.AssignedFlags).Single(x => x.ID == document.ID);
                foreach (var existingFlag in dbDocument.AssignedFlags.ToList())
                {
                    if (!newFlags.Exists(x => x.ID == existingFlag.ID))
                        dbDocument.AssignedFlags.Remove(existingFlag);
                }

                foreach (var newFlag in newFlags)
                {
                    if (!dbDocument.AssignedFlags.Exists(x => x.ID == newFlag.ID))
                        dbDocument.AssignedFlags.Add(allFlags.Single(x => x.ID == newFlag.ID));
                }

                context.SaveChanges();
            }
        }
    }
}