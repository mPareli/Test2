using Emne_7___Arbeidskrav_2.Features.Comments;
using Emne_7___Arbeidskrav_2.Features.Users;

namespace Emne_7___Arbeidskrav_2.Features.Posts;

public class Post
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime DatePosted { get; set; }
    
    public virtual User? User { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}