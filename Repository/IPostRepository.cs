using Nail_Service.Models;

namespace Nail_Service.Repository
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> CreatePostAsync(string userId, Post post);
        Task<Post> UpdatePostAsync(string userId, int id, Post post);
        Task DeletePostAsync(int id);
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId);
    }
}
