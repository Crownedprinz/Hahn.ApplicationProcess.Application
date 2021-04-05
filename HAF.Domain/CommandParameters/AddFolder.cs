using System;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddFolder
    {
        public AddFolder(Folder folder)
        {
            Folder = folder ?? throw new ArgumentNullException(nameof(folder));
        }

        public Folder Folder { get; }
    }
}