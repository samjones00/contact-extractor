using ContactExtractor.Core.Interfaces;

namespace ContactExtractor.Core.Services
{
    public class SearchService(HttpClient httpClient) : ISearchService
    {
        public async Task<string> Search(string location, CancellationToken cancellationToken)
        {
            var values = new[]
            {
                new KeyValuePair<string, string>("did", "Select area of law"),
                new KeyValuePair<string, string>("location", location)
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
            {
                Content = new FormUrlEncodedContent(values)
            };
    
            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}