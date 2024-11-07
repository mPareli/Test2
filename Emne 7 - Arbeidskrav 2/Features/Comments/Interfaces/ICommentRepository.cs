namespace Emne_7___Arbeidskrav_2.Features.Comments.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetAllCommentsAsync(int page, int size);
    Task<Comment?> GetCommentByIdAsync(int commentId);
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    Task<Comment> CreateCommentAsync(Comment comment);
    Task<Comment?> UpdateCommentAsync(Comment comment);
    Task<bool> DeleteCommentAsync(int commentId);
}