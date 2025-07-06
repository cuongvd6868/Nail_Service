using Microsoft.AspNetCore.Mvc;
using Nail_Service.DTOs.NailSalonDto;
using Nail_Service.Mappers;
using Nail_Service.Repository;

namespace Nail_Service.Controllers
{
    [Route("api/nailsalon")]
    [ApiController]
    public class NailSalonController : ControllerBase
    {
        private readonly INailSalonRepository _nailSalonRepository;

        public NailSalonController(INailSalonRepository nailSalonRepository)
        {
            _nailSalonRepository = nailSalonRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNailSalons()
        {
            var nailSalons = await _nailSalonRepository.GetAllNailSalonsAsync();
            var nailSalonDtos = nailSalons.Select(n => n.ToNailSalonViewDto());
            if (nailSalons == null || !nailSalons.Any())
            {
                return NotFound(new { Message = "No nail salons found." });
            }
            return Ok(nailSalonDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNailSalonById(int id)
        {
            var nailSalon = await _nailSalonRepository.GetNailSalonByIdAsync(id);
            if (nailSalon == null)
            {
                return NotFound(new { Message = $"Nail salon with ID {id} not found." });
            }
            return Ok(nailSalon);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNailSalon([FromBody] CreateNailSalonDto nailSalonDto)
        {
            if(nailSalonDto == null)
            {
                return BadRequest(new { Message = "Nail salon data is required." });
            }
            var nailSalonModel = nailSalonDto.ToNailSalonFromCreateDto();
            await _nailSalonRepository.CreateNailSalonAsync(nailSalonModel);
            return CreatedAtAction(nameof(GetNailSalonById), new { id = nailSalonModel.Id }, nailSalonModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNailSalon(int id, [FromBody] UpdateNailSalonDto nailSalonDto)
        {
            if (nailSalonDto == null || id <= 0)
            {
                return BadRequest(new { Message = "Invalid nail salon data." });
            }
            var naiSalonModel = nailSalonDto.ToNailSalonFromUpdateDto();
            var updatedNailSalon = await _nailSalonRepository.UpdateNailSalonAsync(id, naiSalonModel);
            if (updatedNailSalon == null)
            {
                return NotFound(new { Message = $"Nail salon with ID {id} not found." });
            }
            return Ok(updatedNailSalon);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNailSalon(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Message = "Invalid nail salon ID." });
            }
            await _nailSalonRepository.DeleteNailSalonAsync(id);
            return NoContent();
        }
    }
}
