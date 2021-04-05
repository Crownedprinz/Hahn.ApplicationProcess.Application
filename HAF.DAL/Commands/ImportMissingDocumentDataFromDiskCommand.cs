using System.Diagnostics;
using System.IO;
using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;

namespace  HAF.DAL.Commands
{
    public class ImportMissingDocumentDataFromDiskCommand : ICommand<ImportMissingDocumentDataFromDisk>
    {
        public void Execute(ImportMissingDocumentDataFromDisk parameters)
        {
            using (var context = new DatabaseContext())
            {
                context.Database.CommandTimeout = 300;
                var files = context.Documents.AsQueryable()
                    .Where(x => x.ContextDataContentType != null && x.DocumentData == null)
                    .ToArray();

                foreach (var file in files)
                {
                    var filePath = file.ContextDataContentType;
                    file.DocumentData = new DocumentData(File.ReadAllBytes(filePath));
                    file.ContextDataContentType = null;
                    Debug.WriteLine("Updating document {0}", file);
                    context.SaveChanges();
                }
            }
        }
    }
}