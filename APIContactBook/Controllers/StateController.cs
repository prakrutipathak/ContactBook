using APIContactBook.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }
        [HttpGet("GetAllStates")]
        public IActionResult GetAllStates()
        {
            var response = _stateService.GetAllStates();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetStatesByCountryId/{id}")]
        public IActionResult GetStatesByCountryId(int id)
        {
            var response = _stateService.GetStatesByCountryId(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
