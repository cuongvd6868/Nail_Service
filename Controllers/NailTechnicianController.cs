using Microsoft.AspNetCore.Mvc;
using Nail_Service.DTOs.NailTechnicianDto;
using Nail_Service.Extensions;
using Nail_Service.Mappers;
using Nail_Service.Repository;

namespace Nail_Service.Controllers
{
    [Route("api/nailtechnician")]
    [ApiController]
    public class NailTechnicianController : ControllerBase
    {
        private readonly INailTechnicianRepository _nailTechnicianRepository;
        public NailTechnicianController(INailTechnicianRepository nailTechnicianRepository)
        {
            _nailTechnicianRepository = nailTechnicianRepository ?? throw new ArgumentNullException(nameof(nailTechnicianRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNailTechnicians()
        {
            var nailTechnicians = await _nailTechnicianRepository.GetAllTechniciansAsync();
            return Ok(nailTechnicians);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNailTechnicianById(int id)
        {
            var nailTechnician = await _nailTechnicianRepository.GetTechnicianByIdAsync(id);
            return Ok(nailTechnician);
        }

        [HttpGet("salon/{salonId}")]
        public async Task<IActionResult> GetTechniciansIdBySalonId(int salonId)
        {
            var nailTechnicians = await _nailTechnicianRepository.GetTechniciansBySalonIdAsync(salonId);
            return Ok(nailTechnicians);
        }

        [HttpGet("nailSalon/{salonId}")]
        public async Task<IActionResult> GetTechniciansBySalonId(int salonId)
        {
            var nailTechnicians = await _nailTechnicianRepository.getNailTechniciansBySalonIdAsync(salonId);
            return Ok(nailTechnicians);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNailTechnician(int id)
        {
            try
            {
                await _nailTechnicianRepository.DeleteTechnicianAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNailTechnician([FromBody] CreateNailTechnicianDto createDto)
        {
            if (createDto == null)
            {
                return BadRequest("Technician data is required.");
            }
            var technician = createDto.ToNailTechnicianFromCreateDto();
            var createdTechnician = await _nailTechnicianRepository.CreateTechnicianAsync(technician);
            if (createdTechnician == null)
            {
                return StatusCode(500, "An error occurred while creating the technician.");
            }
            var technicianViewDto = createdTechnician.ToNailTechnicianViewDto();
            return CreatedAtAction(nameof(GetNailTechnicianById), new { id = technicianViewDto.Id }, technicianViewDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNailTechnician([FromRoute] int id, [FromBody] UpdateNailTechnicianDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Technician data is required.");
            }
            var userId = User.GetUserId();
            var technician = updateDto.ToNailTechnicianFromUpdate();
            var updatedTechnician = await _nailTechnicianRepository.UpdateTechnicianAsync(userId, id, technician);
            if (updatedTechnician == null)
            {
                return NotFound($"Technician with ID {id} not found or not accessible by this user.");
            }
            var technicianViewDto = updatedTechnician.ToNailTechnicianViewDto();
            return Ok(technicianViewDto);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTechnicianStatus([FromRoute] int id, [FromBody] StatusNailTechnicianDto statusDto)
        {
            if (statusDto == null)
            {
                return BadRequest("Status data is required.");
            }
            var userId = User.GetUserId();
            var statusModel = statusDto.ToUpdateStatusDto();
            var success = await _nailTechnicianRepository.UpdateStatusAsync(userId, id, statusModel);
            if (!success)
            {
                return NotFound($"Technician with ID {id} not found or not accessible by this user.");
            }
            return NoContent();
        }




    }
}
