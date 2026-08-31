using HtmlAgilityPack;
using HTMLQuestPDF.Extensions;

namespace HTMLToQPDF.Tests
{
    /// <summary>
    /// Regression tests for <c>TryGetLink</c>. It walks up the ancestor chain, so a
    /// text node inside an anchor must resolve to that anchor's href, and a node
    /// without any anchor above it must terminate instead of looping forever.
    /// </summary>
    public class HtmlNodeExtensionsTests
    {
        private static HtmlNode Parse(string html, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.SelectSingleNode(xpath);
        }

        [Fact]
        public void Returns_href_of_the_anchor_itself()
        {
            var node = Parse("<p><a href=\"https://example.com\">link</a></p>", "//a");

            Assert.True(node.TryGetLink(out var url));
            Assert.Equal("https://example.com", url);
        }

        [Fact]
        public void Returns_href_of_an_ancestor_anchor()
        {
            var node = Parse("<p><a href=\"https://example.com\"><b>link</b></a></p>", "//b/text()");

            Assert.True(node.TryGetLink(out var url));
            Assert.Equal("https://example.com", url);
        }

        [Fact]
        public void Returns_false_when_no_anchor_is_present()
        {
            var node = Parse("<p><b>plain</b></p>", "//b/text()");

            Assert.False(node.TryGetLink(out var url));
            Assert.Equal("", url);
        }
    }
}
