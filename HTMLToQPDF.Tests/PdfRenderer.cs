using HTMLQuestPDF;
using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HTMLToQPDF.Tests
{
    /// <summary>
    /// Renders HTML to a PDF byte array the same way a consumer would, so the
    /// tests exercise the real QuestPDF pipeline rather than a stubbed one.
    /// </summary>
    internal static class PdfRenderer
    {
        static PdfRenderer()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] Render(string html, Action<HTMLDescriptor>? configure = null)
        {
            return Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Content().HTML(descriptor =>
                    {
                        descriptor.SetHtml(html);
                        configure?.Invoke(descriptor);
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>A PDF always starts with the %PDF- magic bytes.</summary>
        public static void AssertIsPdf(byte[] bytes)
        {
            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 500, $"Expected a non-trivial PDF, got {bytes.Length} bytes.");
            Assert.Equal("%PDF-", System.Text.Encoding.ASCII.GetString(bytes, 0, 5));
        }
    }
}
