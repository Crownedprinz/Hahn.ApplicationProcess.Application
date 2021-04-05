using System;

namespace HAF.Domain.Entities
{
    public class DocumentData : Entity
    {
        public DocumentData()
        {
        }

        public DocumentData(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public byte[] Data { get; set; }
    }
}