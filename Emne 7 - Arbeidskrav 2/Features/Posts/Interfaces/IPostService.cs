using System.Security.Claims;
using Emne_7___Arbeidskrav_2.Features.Posts.DTO;
using Emne_7___Arbeidskrav_2.Features.Users;

namespace Emne_7___Arbeidskrav_2.Features.Posts.Interfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetAllPostsAsync(int page, int size);
    Task<Post?> GetPostByIdAsync(int postId);
    Task<ServiceResult<Post>> CreatePostAsync(PostCreateDTO postDto, ClaimsPrincipal currentUser);
    Task<ServiceResult<Post>> UpdatePostAsync(int postId, PostUpdateDTO postDto);
    Task<ServiceResult> DeletePostAsync(int postId);
    Task<bool> IsUserAuthorizedAsync(ClaimsPrincipal currentUser, int postId);
}