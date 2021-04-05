using System;

namespace HAF.Domain.Entities
{
    public class BackgroundJob : Entity
    {
        public JobStatus JobStatus { get; set; } = JobStatus.Enqueued;
        public string ResultJson { get; set; }

        public Type ResultType
        {
            get => ResultTypeString != null ? Type.GetType(ResultTypeString) : null;
            set => ResultTypeString = value?.AssemblyQualifiedName;
        }

        public string ResultTypeString { get; set; }
    }
}