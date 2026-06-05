using ContactExtractor.Core.Models;

namespace ContactExtractor.Core.Services
{
    public class SearchService(HttpClient httpClient)
    {
        public async Task<string> Search(string location)
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
    
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}