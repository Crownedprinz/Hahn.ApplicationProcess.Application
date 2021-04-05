using System;
using System.Collections.Generic;

namespace HAF.Domain.Entities
{
    public class Document : NamedEntity
    {
        public List<DocumentFlag> AssignedFlags { get; set; }
        public DocumentData ContextData { get; set; }
        public string ContextDataContentType { get; set; }
        public string ContextFileExtension { get; set; }
        public DateTime? Date { get; set; }
        public DebitorCreditor DebitorCreditor { get; set; }
        public int? DebitorCreditorID { get; set; }
        public Direction? Direction { get; set; }
        public DocumentData DocumentData { get; set; }
        public string DocumentDataContentType { get; set; }
        public string DocumentFileExtension { get; set; }
        public Folder Folder { get; set; }
        public int? FolderID { get; set; }

        public override string ToString()
        {
            var result = string.Empty;

            if (Date != null)
                result += Date.Value.ToString("yyyy-MM-dd");

            if (Direction != null)
            {
                if (result.Length > 0)
                    result += " - ";
                result += Direction.Value;
            }

            if (result.Length > 0)
                result += " - ";

            result += $"{Name}{DocumentFileExtension}";

            return result;
        }
    }
}