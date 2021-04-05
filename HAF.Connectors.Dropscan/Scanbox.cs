using System.Collections.Generic;

namespace  HAF.Connectors.Dropscan
{
    public class Scanbox
    {
        public bool AutoOpen { get; set; }
        public int Id { get; set; }
        public string Number { get; set; }
        public List<Recipient> Recipients { get; set; }
    }
}