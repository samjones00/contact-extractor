using ContactExtractor.Api.Models;
using ContactExtractor.Api.Validators;
using ContactExtractor.Core.Interfaces;
using ContactExtractor.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactExtractor.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for searching contact information.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    [Tags("Solicitors")]
    public class SolicitorsController(ISearchService searchService, IHtmlContactParser htmlContactParser, Settings settings, SearchRequestValidator validator) : ControllerBase
    {
        /// <summary>
        /// Searches for solicitors in a given location and extracts contact information.
        /// </summary>
        /// <param name="request">The search request containing the location to search for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of contacts found in the specified location.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /solicitors
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
        [ProducesResponseType<SearchResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search([FromBody] SearchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    return ValidationProblem(ModelState);
                }

                var html = await searchService.Search(request.Location, cancellationToken);
                var contacts = htmlContactParser.Parse(html);

                return Ok(new SearchResponse(request.Location, contacts));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Gets the list of supported locations.
        /// </summary>
        [HttpGet(nameof(Locations))]
        [ProducesResponseType<List<string>>(StatusCodes.Status200OK)]
        public IActionResult Locations() => Ok(settings.AllowedLocations.OrderBy(x => x));
    }
}
