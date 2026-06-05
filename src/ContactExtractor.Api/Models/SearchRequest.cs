using System.ComponentModel.DataAnnotations;

namespace ContactExtractor.Api.Models
{
    /// <summary>
    /// Represents a search request for finding contacts in a specific location.
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Gets or sets the location to search for (e.g., "london", "manchester").
        /// </summary>
        /// <remarks>This field is required and must not be null or empty.</remarks>
        [Required]
        public string Location { get; set; }
    }
}