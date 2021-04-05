using System;
using HAF.Domain;

namespace HAF.Seeder
{
    public class ExecuteSeeder : IApplicationStartUpHandler
    {
        private readonly ISeeder _seeder;

        public ExecuteSeeder(ISeeder seeder)
        {
            _seeder = seeder ?? throw new ArgumentNullException(nameof(seeder));
        }

        public void Handle()
        {
            _seeder.Seed();
        }
    }
}