using System;
using HAF.Domain.Entities;

namespace  HAF.Connectors.Dropscan
{
    public class Mailing
    {
        public DateTime? DownloadedAt { get; set; }
        public DateTime? ForwardedAt { get; set; }
        public DateTime? ForwardRequestedAt { get; set; }
        public DateTime ReceivedAt { get; set; }

        public int RecepientId
        {
            get => RecipientId;
            set => RecipientId = value;
        }

        public int RecipientId { get; set; }
        public int ScanboxId { get; set; }
        public DateTime? ScannedAt { get; set; }
        public DateTime? ScanRequestedAt { get; set; }
        public DropscanMailingStatus Status { get; set; }
        public string Uuid { get; set; }
    }
}