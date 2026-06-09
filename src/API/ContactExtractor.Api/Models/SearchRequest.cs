using System.ComponentModel.DataAnnotations;

namespace ContactExtractor.Api.Models
{
    /// <summary>
    /// Represents a search request for finding contacts in a specific location.
    /// </summary>
    public record SearchRequest([Required] string Location = "");
}