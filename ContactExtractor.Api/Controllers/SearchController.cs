using ContactExtractor.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactExtractor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        
        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Contact> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Contact("Sam","hioo","jpj","jllj"))
            .ToArray();
        }
    }
}
