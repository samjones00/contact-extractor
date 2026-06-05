using System.Xml;
using HtmlAgilityPack;

namespace ContactExtractor.Api.Services
{
    public class SearchService(HttpClient httpClient)
    {
        public async Task<string> GetBodyAsync(string location, string url)
        {
            var did = "Select+area+of+law";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0\r\n");

            var bodyString = $"did={did}&location={location}";
            request.Content = new StringContent(bodyString, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}