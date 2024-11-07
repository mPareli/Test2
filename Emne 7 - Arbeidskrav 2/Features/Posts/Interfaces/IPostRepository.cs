namespace Emne_7___Arbeidskrav_2.Features.Posts.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync(int page, int size);
    Task<Post?> GetPostByIdAsync(int postId);
    Task<Post> CreatePostAsync(Post post);
    Task<Post?> UpdatePostAsync(Post post);
    Task<bool> DeletePostAsync(int postId);
}