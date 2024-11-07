using System.Security.Claims;
using Emne_7___Arbeidskrav_2.Features.Comments.DTO;
using Emne_7___Arbeidskrav_2.Features.Users;

namespace Emne_7___Arbeidskrav_2.Features.Comments.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentRequestDTO>> GetAllCommentsAsync(int page, int size);
    Task<CommentRequestDTO?> GetCommentByIdAsync(int commentId);
    Task<IEnumerable<CommentRequestDTO>> GetCommentsByPostIdAsync(int postId);
    Task<ServiceResult<CommentRequestDTO>> CreateCommentAsync(int postId, CommentCreateDTO commentDto, ClaimsPrincipal currentUser);
    Task<ServiceResult<CommentRequestDTO>> UpdateCommentAsync(int commentId, CommentUpdateDTO commentDto);
    Task<ServiceResult> DeleteCommentAsync(int commentId);
    Task<bool> IsUserAuthorizedAsync(ClaimsPrincipal currentUser, int commentId);
}