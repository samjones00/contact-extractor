using ContactExtractor.Api.Models;
using ContactExtractor.Api.Validators;
using ContactExtractor.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactExtractor.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for searching contact information.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Tags("Contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly IHtmlContactParser _htmlContactParser;
        private readonly SearchRequestValidator _validator;

        public ContactsController(ISearchService searchService, IHtmlContactParser htmlContactParser, SearchRequestValidator validator)
        {
            _searchService = searchService;
            _htmlContactParser = htmlContactParser;
            _validator = validator;
        }

        /// <summary>
        /// Searches for solicitors in a given location and extracts contact information.
        /// </summary>
        /// <param name="request">The search request containing the location to search for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
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
        public async Task<IActionResult> Search([FromBody] SearchRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return ValidationProblem(ModelState);
            }

            var html = await _searchService.Search(request.Location, cancellationToken);
            var contacts = _htmlContactParser.ExtractContacts(html);

            return Ok(new SearchResponse(request.Location, contacts));
        }
    }
}
