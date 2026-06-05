using ContactExtractor.Core.Models;

namespace ContactExtractor.Core.Services
{
    public class SearchService(HttpClient httpClient, SearchSettings settings)
    {
        public async Task<string> Search(string location)
        {
            var did = "Select+area+of+law";

            using var request = new HttpRequestMessage(HttpMethod.Get, settings.Url);
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0\r\n");
            request.Content = new StringContent($"did={did}&location={location}", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}