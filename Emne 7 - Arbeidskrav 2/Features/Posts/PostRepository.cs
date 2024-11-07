using Emne_7___Arbeidskrav_2.Data;
using Emne_7___Arbeidskrav_2.Features.Posts.Interfaces;

namespace Emne_7___Arbeidskrav_2.Features.Posts;

public class PostRepository : IPostRepository
{
    private readonly StudentBloggDbContext _context;

    public PostRepository(StudentBloggDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync(int page, int size)
    {
        return await _context.Posts
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int postId)
    {
        return await _context.Posts
            .Include(p => p.User)        
            .Include(p => p.Comments)     
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post?> UpdatePostAsync(Post post)
    {
        var existingPost = await _context.Posts.FindAsync(post.Id);
        if (existingPost == null)
            return null;

        existingPost.Title = post.Title;
        existingPost.Content = post.Content;

        _context.Posts.Update(existingPost);
        await _context.SaveChangesAsync();
        return existingPost;
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
            return false;

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }
}