using ContactExtractor.Api.Models;
using ContactExtractor.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactExtractor.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for searching and extracting contact information.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Tags("Contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly SearchService _searchService;
        private readonly HtmlContactParser _htmlContactParser;

        public ContactsController(SearchService searchService, HtmlContactParser htmlContactParser)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _htmlContactParser = htmlContactParser ?? throw new ArgumentNullException(nameof(htmlContactParser));
        }

        /// <summary>
        /// Searches for solicitors in a given location and extracts contact information.
        /// </summary>
        /// <param name="request">The search request containing the location to search for.</param>
        /// <returns>A list of contacts found in the specified location.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /contacts
        ///     {
        ///       "location": "london"
        ///     }
        ///
        /// Sample response:
        ///
        ///     {
        ///       "location": "london",
        ///       "count": 42,
        ///       "results": [
        ///         {
        ///           "name": "Example Solicitors",
        ///           "email": "contact@example.com",
        ///           "phone": "+44 123 456 7890"
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        [HttpPost(Name = nameof(Search))]
        [Produces("application/json")]
        [ProducesResponseType<SearchResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SearchResponse> Search([FromBody] SearchRequest request)
        {
            var location = request?.Location ?? throw new ArgumentNullException(nameof(request.Location));

            var html = await _searchService.Search(location);
            var contacts = _htmlContactParser.ExtractContacts(html);    

            return new SearchResponse(location, contacts);
        }
    }
}
