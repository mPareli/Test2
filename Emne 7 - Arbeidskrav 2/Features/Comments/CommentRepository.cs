using Emne_7___Arbeidskrav_2.Data;
using Emne_7___Arbeidskrav_2.Features.Comments.Interfaces;

namespace Emne_7___Arbeidskrav_2.Features.Comments;

public class CommentRepository : ICommentRepository
{
    private readonly StudentBloggDbContext _context;

    public CommentRepository(StudentBloggDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsAsync(int page, int size)
    {
        return await _context.Comments
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int commentId)
    {
        return await _context.Comments
            .Include(c => c.Post)      // Optional: Include the related Post if needed
            .Include(c => c.User)      // Optional: Include the User who made the comment if needed
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> UpdateCommentAsync(Comment comment)
    {
        var existingComment = await _context.Comments.FindAsync(comment.Id);
        if (existingComment == null)
            return null;

        existingComment.Content = comment.Content;
        
        _context.Comments.Update(existingComment);
        await _context.SaveChangesAsync();
        return existingComment;
    }

    public async Task<bool> DeleteCommentAsync(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            return false;

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
}