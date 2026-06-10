using System.Text.RegularExpressions;
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

            return htmlDoc.DocumentNode
                .Descendants("div")
                .Where(d => d.HasClass("result-item"))
                .Select(ToContact)
                .ToList();
        }

        private static Models.Contact ToContact(HtmlNode item) =>
            new(
                ExtractName(item),
                ExtractAddress(item),
                ExtractTelephone(item),
                ExtractDescription(item),
                ExtractWebsite(item)
            );

        private static string ExtractName(HtmlNode item)
        {
            var h2 = item.Find("div.top-holder")?.Find("span.h2") ?? item.Find("span.h2");
            if (h2 == null) return string.Empty;

            var textChild = h2.ChildNodes.FirstOrDefault(n => n.NodeType == HtmlNodeType.Text);
            return CleanWhitespace(textChild?.InnerText ?? h2.InnerText ?? string.Empty);
        }

        private static string ExtractAddress(HtmlNode item) =>
            InnerTextOrEmpty(item.Find("address"));

        private static string ExtractTelephone(HtmlNode item) =>
            item.Find(a => a.HrefStartsWith("tel:"))?.InnerText.Trim()
            ?? item.Find("div.phone-block")?.Find("a")?.InnerText.Trim()
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
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return CollapseSpacesRegex.Replace(
                NonAlphaNumRegex.Replace(
                    NewlineRegex.Replace(input, " "), string.Empty), " ").Trim();
        }

        private static string InnerTextOrEmpty(HtmlNode? node) =>
            node?.InnerText is string text ? CleanWhitespace(text) : string.Empty;
    }

    internal static class HtmlNodeExtensions
    {
        internal static bool HasClass(this HtmlNode node, string className) =>
            node.GetAttributeValue("class", "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Contains(className);

        internal static HtmlNode? Find(this HtmlNode node, string selector)
        {
            var parts = selector.Split('.', 2);
            return parts.Length == 1
                ? node.Descendants(parts[0]).FirstOrDefault()
                : node.Descendants(parts[0]).FirstOrDefault(n => n.HasClass(parts[1]));
        }

        internal static HtmlNode? Find(this HtmlNode node, Func<HtmlNode, bool> predicate) =>
            node.Descendants().FirstOrDefault(predicate);

        internal static bool HrefStartsWith(this HtmlNode node, string prefix) =>
            (node.GetAttributeValue("href", "") ?? "")
                .StartsWith(prefix, StringComparison.OrdinalIgnoreCase);

        internal static bool IsTargetBlank(this HtmlNode node) =>
            node.GetAttributeValue("target", "") == "_blank";
    }
}
