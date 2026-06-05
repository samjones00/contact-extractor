using ContactExtractor.Core.Models;

namespace ContactExtractor.Core.Services
{
    public class SearchService(HttpClient httpClient, SearchSettings settings)
    {
        public async Task<string> Search(string location)
        {
            var values = new[]
            {
                new KeyValuePair<string, string>("did", "Select area of law"),
                new KeyValuePair<string, string>("location", location)
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, settings.Url)
            {
                Content = new FormUrlEncodedContent(values)
            };

            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}