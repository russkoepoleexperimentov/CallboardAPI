namespace Core.Entities
{
    public class Comment : BaseEntity
    {
        public virtual User Author { get; set; } = null!;
        public virtual Advertisement Advertisement { get; set; } = null!;
        public virtual Comment? Parent { get; set; }
        public Guid? RootId { get; set; }
        public string? Text { get; set; }
        public bool Edited { get; set; }
    }
}
