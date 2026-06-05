using ContactExtractor.Core.Models;
using ContactExtractor.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactExtractor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly SearchService searchService;
        private readonly HtmlContactParser htmlContactParser;

        public ContactsController(ILogger<ContactsController> logger, SearchService searchService, HtmlContactParser htmlContactParser)
        {
            _logger = logger;
            this.searchService = searchService;
            this.htmlContactParser = htmlContactParser;
        }

        [HttpGet(Name = "GetContacts")]
        public async Task<IEnumerable<Contact>> Get()
        {
            var html = await searchService.Search("london");
            var contacts = htmlContactParser.ExtractContacts(html);
            return contacts;
        }
    }
}
