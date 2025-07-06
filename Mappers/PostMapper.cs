using Microsoft.Identity.Client;
using Nail_Service.DTOs.PostDto;
using Nail_Service.Models;

namespace Nail_Service.Mappers
{
    public static class PostMapper
    {
        public static PostViewDto ToPostViewDto(this Post post)
        {
            if (post == null) return null;
            return new PostViewDto
            {
                Id = post.Id,
                Title = post.Title,
                ContentMarkdown = post.ContentMarkdown,
                ContentHtml = post.ContentHtml,
                ImageData = post.ImageData,
                Status = post.Status,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                AuthorId = post.AuthorId
            };
        }

        public static Post ToPostCreateDto(this CreatePostDto createPostDto)
        {
            if (createPostDto == null) return null;
            return new Post
            {
                Title = createPostDto.Title,
                ContentMarkdown = createPostDto.ContentMarkdown,
                ContentHtml = createPostDto.ContentHtml,
                ImageData = createPostDto.ImageData,
                Status = createPostDto.Status,
                CreatedAt = DateTime.UtcNow,
                AuthorId = createPostDto.AuthorId
            };
        }

        public static Post ToPostUpdateDto(this UpdatePostDto updatePostDto)
        {
            if(updatePostDto == null) return null;
            return new Post
            {
                Id = updatePostDto.Id,
                Title = updatePostDto.Title,
                ContentMarkdown = updatePostDto.ContentMarkdown,
                ContentHtml = updatePostDto.ContentHtml,
                ImageData = updatePostDto.ImageData,
                Status = updatePostDto.Status,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = updatePostDto.AuthorId
            };
        }
    }
}
