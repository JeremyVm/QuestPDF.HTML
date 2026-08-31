using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace HTMLToQPDF.Tests
{
    /// <summary>
    /// Smoke tests: every supported tag must render to a valid PDF without throwing.
    /// These are the tests that catch a breaking QuestPDF release.
    /// </summary>
    public class HtmlRenderingTests
    {
        [Theory]
        // Block level
        [InlineData("<p>Hello world</p>")]
        [InlineData("<h1>H1</h1><h2>H2</h2><h3>H3</h3><h4>H4</h4><h5>H5</h5><h6>H6</h6>")]
        [InlineData("<div>Block</div>")]
        [InlineData("<hr>")]
        [InlineData("<blockquote>Quoted text</blockquote>")]
        [InlineData("<pre>  preformatted\n  text</pre>")]
        // Inline formatting
        [InlineData("<p><strong>bold</strong> <em>italic</em> <u>underline</u> <s>strike</s></p>")]
        [InlineData("<p><span>span</span> <code>code</code> <mark>mark</mark></p>")]
        [InlineData("<p><abbr>abbr</abbr> <kbd>Ctrl</kbd> <samp>out</samp> <var>x</var> <q>quote</q></p>")]
        [InlineData("<p>line one<br>line two</p>")]
        [InlineData("<p><a href=\"https://example.com\">link</a></p>")]
        // Lists
        [InlineData("<ul><li>one</li><li>two</li></ul>")]
        [InlineData("<ol><li>one</li><li>two</li></ol>")]
        [InlineData("<ul><li>outer<ul><li>inner</li></ul></li></ul>")]
        // Tables
        [InlineData("<table><tr><th>A</th><th>B</th></tr><tr><td>1</td><td>2</td></tr></table>")]
        [InlineData("<table><thead><tr><th>H</th></tr></thead><tbody><tr><td>C</td></tr></tbody></table>")]
        public void Renders_supported_tag_to_valid_pdf(string html)
        {
            PdfRenderer.AssertIsPdf(PdfRenderer.Render(html));
        }

        [Theory]
        [InlineData("<p style=\"color: #ff0000\">red</p>")]
        [InlineData("<p style=\"color: rgb(0, 128, 0)\">green</p>")]
        [InlineData("<p style=\"color: rgba(0, 0, 255, 0.5)\">blue</p>")]
        [InlineData("<p style=\"background-color: yellow\">highlight</p>")]
        [InlineData("<p style=\"font-size: 18px; font-weight: bold\">big</p>")]
        [InlineData("<p style=\"font-style: italic; text-decoration: underline\">styled</p>")]
        [InlineData("<div style=\"padding: 10px; margin: 5px; border: 1px solid black\">boxed</div>")]
        [InlineData("<div style=\"width: 200pt; height: 50pt; text-align: center\">sized</div>")]
        [InlineData("<p style=\"font-size: 1.5em; letter-spacing: 2px; line-height: 1.5\">spaced</p>")]
        public void Renders_inline_css_to_valid_pdf(string html)
        {
            PdfRenderer.AssertIsPdf(PdfRenderer.Render(html));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("plain text without tags")]
        [InlineData("<p>unclosed paragraph")]
        [InlineData("<p style=\"color: not-a-color; font-size: nonsense\">bad css</p>")]
        [InlineData("<p>&nbsp;&amp;&lt;&gt;&quot;</p>")]
        public void Does_not_throw_on_edge_case_input(string html)
        {
            var exception = Record.Exception(() => PdfRenderer.Render(html));
            Assert.Null(exception);
        }

        [Fact]
        public void Renders_multi_page_document()
        {
            var html = string.Concat(Enumerable.Repeat("<p>Filler paragraph for pagination.</p>", 300));
            PdfRenderer.AssertIsPdf(PdfRenderer.Render(html));
        }

        [Fact]
        public void Applies_custom_text_style_for_tag()
        {
            var bytes = PdfRenderer.Render(
                "<p>styled by descriptor</p>",
                descriptor => descriptor.SetTextStyleForHtmlElement("p", TextStyle.Default.FontSize(20)));

            PdfRenderer.AssertIsPdf(bytes);
        }

        [Fact]
        public void Applies_custom_container_style_for_tag()
        {
            var bytes = PdfRenderer.Render(
                "<p>padded by descriptor</p>",
                descriptor => descriptor.SetContainerStyleForHtmlElement("p", c => c.Padding(10)));

            PdfRenderer.AssertIsPdf(bytes);
        }

        [Fact]
        public void Applies_list_vertical_padding()
        {
            var bytes = PdfRenderer.Render(
                "<ul><li>one</li><li>two</li></ul>",
                descriptor => descriptor.SetListVerticalPadding(5, Unit.Point));

            PdfRenderer.AssertIsPdf(bytes);
        }
    }
}
