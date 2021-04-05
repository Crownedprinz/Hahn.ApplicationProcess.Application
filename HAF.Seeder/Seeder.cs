using System;
using System.Data.Entity;
using System.IO;
using HAF.Connectors.Dropscan;
using HAF.DAL;
using HAF.Domain;
using HAF.Domain.CommandParameters;

namespace HAF.Seeder
{
    public class Seeder : ISeeder
    {
        private readonly ICommand<AddCompanies> _addCompanies;
        private readonly Connector _dropscanConnector;
        private readonly ICommand<ImportMissingDocumentDataFromDisk> _importMissingDocumentDataFromDisk;
        private readonly ICommand<MapDropscanRecipientToCompany> _mapDropscanRecipientToCompany;

        public Seeder(
            ICommand<AddCompanies> addCompanies,
            ICommand<MapDropscanRecipientToCompany> mapDropscanRecipientToCompany,
            ICommand<ImportMissingDocumentDataFromDisk> importMissingDocumentDataFromDisk,
            Connector dropscanConnector)
        {
            _addCompanies = addCompanies ?? throw new ArgumentNullException(nameof(addCompanies));
            _mapDropscanRecipientToCompany = mapDropscanRecipientToCompany ?? throw new ArgumentNullException(nameof(mapDropscanRecipientToCompany));
            _importMissingDocumentDataFromDisk = importMissingDocumentDataFromDisk ?? throw new ArgumentNullException(nameof(importMissingDocumentDataFromDisk));
            _dropscanConnector = dropscanConnector ?? throw new ArgumentNullException(nameof(dropscanConnector));
        }

        public void Seed()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DatabaseContext>());

            Console.WriteLine("Reading file structure from disk...");
            var companiesRoot = @"C:\\Users\\Ademolanj\\Downloads\\test-task\\simple-dms\\companies\\";
            var companies = new[]
            {
                new CompanyFactory(
                    "Sovarto GmbH",
                    Path.Combine(companiesRoot, "Sovarto GmbH", "Ablage"),
                    "Needs payment",
                    "Is tax relevant").Create(),
                new CompanyFactory(
                    "Daniel Hilgarth",
                    Path.Combine(companiesRoot, "Daniel Hilgarth", "Ablage"),
                    "Needs payment").Create()
            };

            Console.WriteLine("Saving companies to database...");
            _addCompanies.Execute(new AddCompanies(companies));

            /*Console.WriteLine("Importing new mailings...");
            _dropscanConnector.ImportNewMailings();

            Console.WriteLine("Mapping Dropscan recipients to companies...");
            _mapDropscanRecipientToCompany.Execute(
                new MapDropscanRecipientToCompany("Sovarto GmbH", "Sovarto GmbH"));
            _mapDropscanRecipientToCompany.Execute(new MapDropscanRecipientToCompany("Daniel Hilgarth", "Daniel Hilgarth"));*/

            Console.WriteLine("Importing document data...");
            _importMissingDocumentDataFromDisk.Execute(new ImportMissingDocumentDataFromDisk());
        }
    }
}