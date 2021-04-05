using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MimeTypes;
using HAF.Domain;
using HAF.Domain.Entities;

namespace HAF.Seeder
{
    public class CompanyFactory
    {
        private readonly string[] _allowedFlags;
        private readonly string _name;
        private readonly string _path;

        public CompanyFactory(string name, string path, params string[] allowedFlags)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _allowedFlags = allowedFlags ?? throw new ArgumentNullException(nameof(allowedFlags));
        }

        public Company Create()
        {
            return new Company
            {
                Name = _name,
                AllowedDocumentFlags = _allowedFlags.Select(x => new DocumentFlag { Name = x }).ToList(),
                DebitorCreditors = Directory.EnumerateDirectories(_path).Select(CreateDebitorCreditor).ToList()
            };
        }

        private static DebitorCreditor CreateDebitorCreditor(string path)
        {
            var result = new DebitorCreditor { Name = Path.GetFileName(path) };
            result.RootFolder = CreateFolder(result, path, "/");
            return result;
        }

        private static Document CreateFile(DebitorCreditor debitorCreditor, string path)
        {
            var result = new Document();
            var fileName = Path.GetFileNameWithoutExtension(path);
            var match = Regex.Match(
                fileName,
                @"((?<year>(19|20)\d{2})(-?(?<month>(0[1-9]|1[0-2]))(-?(?<day>(0[1-9]|[12][0-9]|3[01])))?)?)?( - )?(?<direction>IN|OUT)?( - )?(?<name>.+)?");
            var yearGroup = match.Groups["year"];
            var monthGroup = match.Groups["month"];
            var dayGroup = match.Groups["day"];
            var directionGroup = match.Groups["direction"];
            var nameGroup = match.Groups["name"];
            if (yearGroup.Success)
            {
                int year;
                if (int.TryParse(yearGroup.Value, out year))
                {
                    int month;
                    int day;
                    if (monthGroup.Success && int.TryParse(monthGroup.Value, out month))
                    {
                        if (!dayGroup.Success || !int.TryParse(dayGroup.Value, out day))
                            day = 1;
                    }
                    else
                    {
                        month = 1;
                        day = 1;
                    }

                    try
                    {
                        result.Date = new DateTime(year, month, day);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                    }
                }
            }

            if (directionGroup.Success)
            {
                Direction direction;
                if (Enum.TryParse(directionGroup.Value, true, out direction))
                    result.Direction = direction;
            }

            if (nameGroup.Success)
                result.Name = nameGroup.Value;
            else
                result.Name = Path.GetFileName(Path.GetDirectoryName(path));

            // Hack to allow import of DocumentData at a later time
            result.ContextDataContentType = path;
            result.DocumentFileExtension = Path.GetExtension(path);
            result.DocumentDataContentType = MimeTypeMap.GetMimeType(result.DocumentFileExtension);
            result.DebitorCreditor = debitorCreditor;
            return result;
        }

        private static Folder CreateFolder(DebitorCreditor debitorCreditor, string path) =>
            CreateFolder(debitorCreditor, path, Path.GetFileName(path));

        private static Folder CreateFolder(DebitorCreditor debitorCreditor, string path, string name) =>
            new Folder
            {
                Name = name, Folders = ReadSubFolders(debitorCreditor, path), Files = ReadFiles(debitorCreditor, path)
            };

        private static List<Document> ReadFiles(DebitorCreditor debitorCreditor, string path)
        {
            return Directory.EnumerateFiles(path).Select(x => CreateFile(debitorCreditor, x)).ToList();
        }

        private static List<Folder> ReadSubFolders(DebitorCreditor debitorCreditor, string path)
        {
            return Directory.EnumerateDirectories(path).Select(x => CreateFolder(debitorCreditor, x)).ToList();
        }
    }
}