using System.Text.RegularExpressions;
using ContactExtractor.Core.Interfaces;
using ContactExtractor.Core.Models;
using HtmlAgilityPack;

namespace ContactExtractor.Core.Services
{
    public class HtmlContactParser : IHtmlContactParser
    {
        public List<Contact> Parse(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return htmlDocument.DocumentNode
                .Descendants("div")
                .Where(d => d.HasClass("result-item"))
                .Select(ToContact)
                .ToList();
        }

        private static Contact ToContact(HtmlNode item) =>
            new(
                ExtractName(item),
                ExtractAddress(item),
                ExtractTelephone(item),
                ExtractDescription(item),
                ExtractWebsite(item)
            );

        private static string ExtractName(HtmlNode item)
        {
            var h2 = item.Find("span.h2");
            if (h2 == null) return string.Empty;

            var textChild = h2.ChildNodes.FirstOrDefault(n => n.NodeType == HtmlNodeType.Text);
            return CleanWhitespace(textChild?.InnerText ?? h2.InnerText ?? string.Empty);
        }

        private static string ExtractAddress(HtmlNode item) =>
            InnerTextOrEmpty(item.Find("address"));

        private static string ExtractTelephone(HtmlNode item) =>
            item.Find(a => a.HrefStartsWith("tel:"))?.InnerText.Trim()
            ?? string.Empty;

        private static string ExtractDescription(HtmlNode item) =>
            InnerTextOrEmpty(item.Find("p"));

        private static string ExtractWebsite(HtmlNode item) =>
            item.Find(a => a.IsTargetBlank() && a.HrefStartsWith("http"))
                ?.GetAttributeValue("href", "").Trim()
            ?? string.Empty;

        private static readonly Regex NewlineRegex = new("[\r\n\t]+");
        private static readonly Regex NonAlphaNumRegex = new("[^A-Za-z0-9, ]+");
        private static readonly Regex CollapseSpacesRegex = new(" {2,}");

        private static string CleanWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input)) 
                return string.Empty;

            return CollapseSpacesRegex.Replace(
                NonAlphaNumRegex.Replace(
                    NewlineRegex.Replace(input, " "), string.Empty), " ").Trim();
        }

        private static string InnerTextOrEmpty(HtmlNode? node) =>
            node?.InnerText is string text ? CleanWhitespace(text) : string.Empty;
    }
}