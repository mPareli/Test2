namespace Emne_7___Arbeidskrav_2.Features.Comments.DTO;

public class CommentRequestDTO
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DateCommented { get; set; }
}