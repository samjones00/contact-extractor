using System.Xml;
using HtmlAgilityPack;

namespace ContactExtractor.Api.Services
{
    public class ExtractorService()
    {
        public async Task ExtractContacts(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='top-holder']");
        }
    }
}