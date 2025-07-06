using Microsoft.AspNetCore.Mvc;
using Nail_Service.DTOs.PostDto;
using Nail_Service.Extensions;
using Nail_Service.Mappers;
using Nail_Service.Repository;

namespace Nail_Service.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            if (posts == null || !posts.Any())
            {
                return NotFound(new { Message = "No posts found." });
            }
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { Message = $"Post with ID {id} not found." });
            }
            return Ok(post);
        }

        [HttpPost] 
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {
            if (postDto == null)
            {
                return BadRequest(new { Message = "Post data is required." });  
            }
            var userId = User.GetUserId();
            var postModel = postDto.ToPostCreateDto();
            await _postRepository.CreatePostAsync(userId, postModel);
            return CreatedAtAction(nameof(GetPostById), new { id = postModel.Id }, postModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto postDto)
        {
            var userId = User.GetUserId();
            var postModel = postDto.ToPostUpdateDto();
            await _postRepository.UpdatePostAsync(userId, id, postModel);
            return Ok(postModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postRepository.DeletePostAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
