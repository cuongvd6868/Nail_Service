using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Nail_Service.Extensions;
using Nail_Service.Models;
using Nail_Service.Repository;

namespace Nail_Service.Controllers
{
    [Route("api/nailsalonfavorite")]
    [ApiController]
    public class NailSalonFavoriteController : ControllerBase
    {
        private readonly INailSalonFavoriteRepository _nailSalonFavoriteRepository;
        public NailSalonFavoriteController(INailSalonFavoriteRepository nailSalonFavoriteRepository)
        {
            _nailSalonFavoriteRepository = nailSalonFavoriteRepository ?? throw new ArgumentNullException(nameof(nailSalonFavoriteRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetFavoriteByUser()
        {
            var userId = User.GetUserId();
            var favorites = await _nailSalonFavoriteRepository.GetFavoritesByUserIdAsync(userId);
            if (favorites == null || !favorites.Any())
            {
                return NotFound(new { Message = "No favorite nail salons found." });
            }
            return Ok(favorites);
        }

        [HttpPost("{nailSalonId}")]
        public async Task<IActionResult> AddToFavorite([FromRoute] int nailSalonId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _nailSalonFavoriteRepository.AddToFavoriteAsync(nailSalonId, userId);
            if (result)
            {
                return Ok(new { Message = "Nail salon added to favorites." });
            }
            return BadRequest(new { Message = "Failed to add nail salon to favorites." });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromFavorite([FromRoute] int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _nailSalonFavoriteRepository.RemoveFromFavoriteAsync(id, userId);
            if (result)
            {
                return Ok(new { Message = "Nail salon removed from favorites." });
            }
            return BadRequest(new { Message = "Failed to remove nail salon to favorites." });
        }

        [HttpGet("isfavorite/{nailSalonId}")]
        public async Task<IActionResult> IsFavorite([FromRoute] int nailSalonId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var isFavorite = await _nailSalonFavoriteRepository.IsFavoriteAsync(nailSalonId, userId);
            return Ok(new { IsFavorite = isFavorite });
        }
    }
}
