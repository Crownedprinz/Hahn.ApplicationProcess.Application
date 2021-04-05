using System;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddBackgroundJob
    {
        public AddBackgroundJob(BackgroundJob backgroundJob)
        {
            BackgroundJob = backgroundJob ?? throw new ArgumentNullException(nameof(backgroundJob));
        }

        public BackgroundJob BackgroundJob { get; }
    }
}