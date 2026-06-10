using ContactExtractor.Core.Models;

namespace ContactExtractor.Api.Models;

/// <summary>
/// Represents the response from a solicitor search.
/// </summary>
public record SearchResponse(string Location, IEnumerable<Contact> Results)
{
    /// <summary>
    /// Gets the total number of solicitors found.
    /// </summary>
    public int Count { get; } = Results?.Count() ?? 0;
}