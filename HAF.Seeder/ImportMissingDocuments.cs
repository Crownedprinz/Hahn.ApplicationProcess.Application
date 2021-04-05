using System;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace HAF.Seeder
{
    public class ImportMissingDocuments : ISeeder
    {
        private readonly ICommand<ImportMissingDocumentDataFromDisk> _importMissingDocumentDataFromDisk;

        public ImportMissingDocuments(ICommand<ImportMissingDocumentDataFromDisk> importMissingDocumentDataFromDisk)
        {
            _importMissingDocumentDataFromDisk = importMissingDocumentDataFromDisk ?? throw new ArgumentNullException(nameof(importMissingDocumentDataFromDisk));
        }

        public void Seed()
        {
            Console.WriteLine("Importing document data...");
            _importMissingDocumentDataFromDisk.Execute(new ImportMissingDocumentDataFromDisk());
        }
    }
}