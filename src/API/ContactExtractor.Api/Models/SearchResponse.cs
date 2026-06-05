using ContactExtractor.Core.Models;

namespace ContactExtractor.Api.Models
{
    /// <summary>
    /// Represents the response from a contact search operation.
    /// </summary>
    public class SearchResponse
    {
        /// <summary>
        /// Gets the location that was searched.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Gets the total number of contacts found.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the collection of contacts found in the search result.
        /// </summary>
        public IEnumerable<Contact> Results { get; private set; }

        public SearchResponse(string location, IEnumerable<Contact> results)
        {
            Results = results;
            Location = location;
            Count = results.Count();
        }
    }
}
