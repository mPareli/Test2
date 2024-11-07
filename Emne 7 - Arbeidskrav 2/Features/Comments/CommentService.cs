using System.Security.Claims;
using Emne_7___Arbeidskrav_2.Features.Comments.DTO;
using Emne_7___Arbeidskrav_2.Features.Comments.Interfaces;
using Emne_7___Arbeidskrav_2.Features.Users;

namespace Emne_7___Arbeidskrav_2.Features.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<CommentRequestDTO>> GetAllCommentsAsync(int page, int size)
    {
        var comments = await _commentRepository.GetAllCommentsAsync(page, size);
        return comments.Select(c => new CommentRequestDTO
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Content = c.Content,
            DateCommented = c.DateCommented
        });
    }

    public async Task<CommentRequestDTO?> GetCommentByIdAsync(int commentId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null) return null;

        return new CommentRequestDTO
        {
            Id = comment.Id,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Content = comment.Content,
            DateCommented = comment.DateCommented
        };
    }

    public async Task<IEnumerable<CommentRequestDTO>> GetCommentsByPostIdAsync(int postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return comments.Select(c => new CommentRequestDTO
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Content = c.Content,
            DateCommented = c.DateCommented
        });
    }

    public async Task<ServiceResult<CommentRequestDTO>> CreateCommentAsync(int postId, 
        CommentCreateDTO commentDto, ClaimsPrincipal currentUser)
    {
        var userId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Content = commentDto.Content,
            DateCommented = DateTime.UtcNow
        };

        var createdComment = await _commentRepository.CreateCommentAsync(comment);

        return new ServiceResult<CommentRequestDTO>(new CommentRequestDTO
        {
            Id = createdComment.Id,
            PostId = createdComment.PostId,
            UserId = createdComment.UserId,
            Content = createdComment.Content,
            DateCommented = createdComment.DateCommented
        });
    }

    public async Task<ServiceResult<CommentRequestDTO>> UpdateCommentAsync(int commentId, CommentUpdateDTO commentDto)
    {
        var existingComment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (existingComment == null)
            return ServiceResult<CommentRequestDTO>.Failure("Comment not found.");

        existingComment.Content = commentDto.Content;

        var updatedComment = await _commentRepository.UpdateCommentAsync(existingComment);
        return updatedComment != null ? 
            ServiceResult<CommentRequestDTO>.Success(new CommentRequestDTO
            {
                Id = updatedComment.Id,
                PostId = updatedComment.PostId,
                UserId = updatedComment.UserId,
                Content = updatedComment.Content,
                DateCommented = updatedComment.DateCommented
            }) : 
            ServiceResult<CommentRequestDTO>.Failure("Update failed.");
    }

    public async Task<ServiceResult> DeleteCommentAsync(int commentId)
    {
        var success = await _commentRepository.DeleteCommentAsync(commentId);
        return success ? ServiceResult.Success() : ServiceResult.Failure("Comment deletion failed.");
    }

    public async Task<bool> IsUserAuthorizedAsync(ClaimsPrincipal currentUser, int commentId)
    {
        var userId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        
        return comment != null && comment.UserId == userId;
    }
}