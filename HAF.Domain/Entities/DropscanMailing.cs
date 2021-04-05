using System;
using System.Collections.Generic;

namespace HAF.Domain.Entities
{
    public class DropscanMailing : Entity
    {
        public List<DiscardedDropscanMailingPage> DiscardedPages { get; set; }
        public int? DocumentID { get; set; }
        public DateTime? DownloadedAt { get; set; }
        public DocumentData EnvelopeData { get; set; }
        public DocumentData FileData { get; set; }
        public DateTime? ForwardedAt { get; set; }
        public DateTime? ForwardRequestedAt { get; set; }
        public List<MappedDropscanMailingPage> MappedPages { get; set; }
        public DropscanMailingMappingStatus MappingStatus { get; set; }
        public int? PageCount { get; set; }
        public List<DropscanMailingPage> Pages { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DropscanRecipient Recipient { get; set; }
        public int RecipientID { get; set; }
        public int ScanboxId { get; set; }
        public DateTime? ScannedAt { get; set; }
        public DateTime? ScanRequestedAt { get; set; }
        public DropscanMailingStatus Status { get; set; }
        public string Uuid { get; set; }
    }
}