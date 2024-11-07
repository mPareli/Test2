using System.Security.Claims;
using Emne_7___Arbeidskrav_2.Features.Posts.DTO;
using Emne_7___Arbeidskrav_2.Features.Posts.Interfaces;
using Emne_7___Arbeidskrav_2.Features.Users;

namespace Emne_7___Arbeidskrav_2.Features.Posts;

public class PostService : IPostService
{
     private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync(int page, int size)
    {
        return await _postRepository.GetAllPostsAsync(page, size);
    }

    public async Task<Post?> GetPostByIdAsync(int postId)
    {
        return await _postRepository.GetPostByIdAsync(postId);
    }

    public async Task<ServiceResult<Post>> CreatePostAsync(PostCreateDTO postDto, ClaimsPrincipal currentUser)
    {
        var userId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var post = new Post
        {
            UserId = userId,
            Title = postDto.Title,
            Content = postDto.Content,
            DatePosted = DateTime.UtcNow
        };

        var createdPost = await _postRepository.CreatePostAsync(post);
        return new ServiceResult<Post>(createdPost);
    }

    public async Task<ServiceResult<Post>> UpdatePostAsync(int postId, PostUpdateDTO postDto)
    {
        var existingPost = await _postRepository.GetPostByIdAsync(postId);
        if (existingPost == null)
            return ServiceResult<Post>.Failure("Post not found.");

        existingPost.Title = postDto.Title ?? existingPost.Title;
        existingPost.Content = postDto.Content ?? existingPost.Content;

        var updatedPost = await _postRepository.UpdatePostAsync(existingPost);
        return updatedPost != null ? ServiceResult<Post>.Success(updatedPost) : ServiceResult<Post>.Failure("Update failed.");
    }

    public async Task<ServiceResult> DeletePostAsync(int postId)
    {
        var success = await _postRepository.DeletePostAsync(postId);
        return success ? ServiceResult.Success() : ServiceResult.Failure("Deletion failed.");
    }

    public async Task<bool> IsUserAuthorizedAsync(ClaimsPrincipal currentUser, int postId)
    {
        var userId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var post = await _postRepository.GetPostByIdAsync(postId);
        
        return post != null && post.UserId == userId;
    }
}