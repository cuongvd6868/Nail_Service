using Microsoft.AspNetCore.Mvc;
using Nail_Service.Repository;

namespace Nail_Service.Controllers
{
    [Route("api/nailservice")]
    [ApiController]
    public class NailServiceController : ControllerBase
    {
        private readonly INailServiceDRepository _nailServiceDRepository;

        public NailServiceController(INailServiceDRepository nailServiceDRepository)
        {
            _nailServiceDRepository = nailServiceDRepository ?? throw new ArgumentNullException(nameof(nailServiceDRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNailServices()
        {
            var nailServices = await _nailServiceDRepository.GetAllNailServiceD();
            return Ok(nailServices);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNailServiceById(int id)
        {
            var nailService = await _nailServiceDRepository.GetNailServiceDById(id);
            if (nailService == null)
            {
                return NotFound(new { Message = $"Nail service with ID {id} not found." });
            }
            return Ok(nailService);
        }

        [HttpGet("nailsalon/{id}")]
        public async Task<IActionResult> GetAllServicesBySalonId(int id)
        {
            var nailServices = await _nailServiceDRepository.GetAllServicesBySalonId(id);
            if (nailServices == null || !nailServices.Any())
            {
                return NotFound(new { Message = $"No services found for salon with ID {id}." });
            }
            return Ok(nailServices);
        }

    }
}
