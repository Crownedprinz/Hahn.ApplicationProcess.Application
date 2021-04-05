using System.Diagnostics;
using HAF.CompositionRoot;

namespace HAF.Seeder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());

            if (args == null || args.Length == 0 || args[0].ToLower() != "only-import-missing-data")
                ApplicationStartup.AfterInternalConfiguration += c => c.Register<ISeeder, Seeder>();
            else
                ApplicationStartup.AfterInternalConfiguration += c => c.Register<ISeeder, ImportMissingDocuments>();

            ApplicationStartup.Execute();
        }
    }
}