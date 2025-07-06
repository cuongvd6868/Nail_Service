using Nail_Service.Data;
using Nail_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Nail_Service.Repository.Impl
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Post> CreatePostAsync(string userId, Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            post.AuthorId = userId;
            post.CreatedAt = DateTime.UtcNow;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeletePostAsync(int id)
        {
            var postDelete = await _context.Posts.FindAsync(id);
            if (postDelete == null)
            {
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }
            _context.Posts.Remove(postDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await  _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId)
        {
            return await _context.Posts.Where(p => p.AuthorId == userId).ToListAsync();
        }

        public async Task<Post> UpdatePostAsync(string userId, int id, Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null)
            {
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }
            // Chỉ cho phép cập nhật nếu người dùng là tác giả của bài viết
            if (existingPost.AuthorId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this post.");
            }
            existingPost.Title = post.Title;
            existingPost.ContentMarkdown = post.ContentMarkdown;
            existingPost.ContentHtml = post.ContentHtml;
            existingPost.ImageData = post.ImageData;
            existingPost.Status = post.Status;
            existingPost.UpdatedAt = DateTime.UtcNow;
            _context.Posts.Update(existingPost);
            await _context.SaveChangesAsync();
            return existingPost;

        }
    }
}
