using HTMLQuestPDF.Utils;

namespace HTMLToQPDF.Tests
{
    /// <summary>
    /// Regression tests for HTML preprocessing. The non-breaking-space cases guard
    /// the fix shipped in 1.4.5.
    /// </summary>
    public class HtmlUtilsTests
    {
        private const char Nbsp = ' ';

        [Fact]
        public void Preserves_non_breaking_space_from_entity()
        {
            var result = HTMLUtils.PrepareHTML("<p>a&nbsp;b</p>");

            Assert.Contains(Nbsp, result);
        }

        [Fact]
        public void Preserves_non_breaking_space_in_nested_inline_elements()
        {
            var result = HTMLUtils.PrepareHTML("<p><strong>a&nbsp;</strong><em>&nbsp;b</em></p>");

            Assert.Equal(2, result.Count(c => c == Nbsp));
        }

        [Fact]
        public void Decodes_html_entities()
        {
            var result = HTMLUtils.PrepareHTML("<p>&amp;&lt;&gt;</p>");

            Assert.Contains("&<>", result);
        }

        [Fact]
        public void Collapses_runs_of_regular_whitespace()
        {
            var result = HTMLUtils.PrepareHTML("<p>a     b</p>");

            Assert.Contains("a b", result);
        }

        [Fact]
        public void Removes_whitespace_between_elements()
        {
            var result = HTMLUtils.PrepareHTML("<div>   <p>x</p>   </div>");

            Assert.DoesNotContain(">   <", result);
        }
    }
}
