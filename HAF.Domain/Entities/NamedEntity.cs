namespace HAF.Domain.Entities
{
    public abstract class NamedEntity : Entity
    {
        public string Name { get; set; }
        public override string ToString() => $"{Name} [{ID}]";
    }
}