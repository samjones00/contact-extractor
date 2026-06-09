using ContactExtractor.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactExtractor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Tags("Settings")]
    public class SettingsController : ControllerBase
    {
        private readonly Settings _settings;

        public SettingsController(Settings settings)
        {
            _settings = settings;
        }

        [HttpGet(nameof(Locations))]
        [Produces("application/json")]
        [ProducesResponseType<List<string>>(StatusCodes.Status200OK)]
        public IActionResult Locations()
        {
            return Ok(_settings.AllowedLocations.OrderBy(x => x));
        }
    }
}
