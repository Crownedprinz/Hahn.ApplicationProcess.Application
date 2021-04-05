using System;
using HAF.Domain.Entities;

namespace HAF.Web.Resources
{
    public class DropscanMailingResource : EntityResource
    {
        public DateTime? DownloadedAt { get; set; }
        public Uri Envelope { get; set; }
        public DateTime? ForwardedAt { get; set; }
        public DateTime? ForwardRequestedAt { get; set; }
        public Uri Pdf { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DropscanRecipientResource Recipient { get; set; }
        public int ScanboxId { get; set; }
        public DateTime? ScannedAt { get; set; }
        public DateTime? ScanRequestedAt { get; set; }
        public DropscanMailingMappingStatus MappingStatus { get; set; }
        public string Uuid { get; set; }
    }
}