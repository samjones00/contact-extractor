using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using ContactExtractor.Core.Interfaces;
using HtmlAgilityPack;

namespace ContactExtractor.Core.Services
{
    public class HtmlContactParser : IHtmlContactParser
    {
        public List<Models.Contact> Parse(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var items = htmlDoc.DocumentNode.SelectNodes("//div[contains(concat(' ', normalize-space(@class), ' '), ' result-item ')]");
            var result = new List<Models.Contact>();
            if (items == null) return result;

            foreach (var item in items)
            {
                var h2 = item.SelectSingleNode(".//div[contains(concat(' ', normalize-space(@class),' '),' top-holder ')]//span[contains(concat(' ', normalize-space(@class),' '),' h2 ')]");
                string rawName = string.Empty;
                if (h2 != null)
                {
                    var textChild = h2.ChildNodes.FirstOrDefault(n => n.NodeType == HtmlNodeType.Text);
                    rawName = (textChild != null ? textChild.InnerText : h2.InnerText) ?? string.Empty;
                }
                var name = CleanWhitespace(rawName);

                var addressNode = item.SelectSingleNode(".//address");
                var rawAddress = addressNode?.InnerText ?? string.Empty;
                var address = CleanWhitespace(rawAddress);

                var phoneAnchor = item.SelectSingleNode(".//a[starts-with(normalize-space(@href),'tel:')]");
                var telephone = phoneAnchor != null
                    ? phoneAnchor.InnerText.Trim()
                    : (item.SelectSingleNode(".//div[contains(concat(' ', normalize-space(@class),' '),' phone-block ')]//a")?.InnerText.Trim() ?? string.Empty);

                var descNode = item.SelectSingleNode("./p");
                var rawDescription = descNode?.InnerText ?? string.Empty;
                var description = CleanWhitespace(rawDescription);

                var websiteAnchor = item.SelectSingleNode(".//a[@target='_blank' and starts-with(@href,'http')]");
                var website = websiteAnchor?.GetAttributeValue("href", string.Empty).Trim() ?? string.Empty;

                result.Add(new(name, address, telephone, description, website));
            }

            return result;
        }

        private static string CleanWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var normalized = Regex.Replace(input, "[\r\n\t]+", " ");
            var removedNonAlphaNum = Regex.Replace(normalized, "[^A-Za-z0-9, ]+", string.Empty);

            var collapsed = Regex.Replace(removedNonAlphaNum, " {2,}", " ");

            return collapsed.Trim();
        }
    }
}