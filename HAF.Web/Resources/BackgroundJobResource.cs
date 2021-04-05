using System;

namespace HAF.Web.Resources
{
    public class BackgroundJobResource : EntityResource
    {
        public string JobStatus { get; set; }
        public string ResultJson { get; set; }
        public string ResultType { get; set; }
        public Uri UpdatedStatusLink { get; set; }
    }
}