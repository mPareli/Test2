using Emne_7___Arbeidskrav_2.Features.Posts;
using Emne_7___Arbeidskrav_2.Features.Users;

namespace Emne_7___Arbeidskrav_2.Features.Comments;

public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DateCommented { get; set; }
    
    public virtual Post? Post { get; set; }
    public virtual User? User { get; set; }

}