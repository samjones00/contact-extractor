using HtmlAgilityPack;

namespace ContactExtractor.Core.Services
{
    internal static class HtmlNodeExtensions
    {
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
