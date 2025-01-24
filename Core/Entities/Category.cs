namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public virtual Category? Parent { get; set; }
        public string Name { get; set; } = null!;

    }
}
