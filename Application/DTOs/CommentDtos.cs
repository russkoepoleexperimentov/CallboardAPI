namespace Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? ParentId { get; set; }
        public string? Text { get; set; }
        public bool Edited { get; set; }
    }

    public class CommentCreateUpdateDto
    {
        public string? Text { get; set; }
    }
}
