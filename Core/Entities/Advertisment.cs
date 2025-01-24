namespace Core.Entities
{
    public class Advertisment : BaseEntity
    {
        public virtual Category Category { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public virtual User Author { get; set; } = null!;
    }
}
