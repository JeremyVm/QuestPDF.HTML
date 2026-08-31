using HTMLQuestPDF.Utils;

namespace HTMLToQPDF.Tests
{
    /// <summary>Unit tests for the public CSS parsing surface added in 1.5.0.</summary>
    public class CssParserTests
    {
        public class ParseStyleAttribute
        {
            [Fact]
            public void Parses_multiple_declarations()
            {
                var styles = CssParser.ParseStyleAttribute("color: red; font-size: 12px");

                Assert.Equal(2, styles.Count);
                Assert.Equal("red", styles["color"]);
                Assert.Equal("12px", styles["font-size"]);
            }

            [Fact]
            public void Ignores_whitespace_and_trailing_semicolon()
            {
                var styles = CssParser.ParseStyleAttribute("  color :  red  ;  ");

                Assert.Equal("red", styles["color"]);
            }

            [Fact]
            public void Lookup_is_case_insensitive()
            {
                var styles = CssParser.ParseStyleAttribute("COLOR: red");

                Assert.Equal("red", styles["color"]);
                Assert.Equal("red", styles["Color"]);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            [InlineData("no-colon-here")]
            [InlineData(": missing-property")]
            public void Returns_empty_for_unusable_input(string? input)
            {
                Assert.Empty(CssParser.ParseStyleAttribute(input));
            }
        }

        public class ParseLength
        {
            [Theory]
            [InlineData("10pt", 10f)]
            [InlineData("10", 10f)]      // no unit -> points
            [InlineData("16px", 12f)]    // 1px = 0.75pt
            [InlineData("1in", 72f)]
            [InlineData("1cm", 28.3465f)]
            [InlineData("1mm", 2.83465f)]
            [InlineData("2em", 24f)]     // 12pt base
            [InlineData("2rem", 24f)]
            [InlineData("-5pt", -5f)]
            [InlineData("1.5pt", 1.5f)]
            public void Converts_unit_to_points(string value, float expected)
            {
                Assert.Equal(expected, CssParser.ParseLength(value)!.Value, 3);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("auto")]
            [InlineData("inherit")]
            [InlineData("initial")]
            [InlineData("nonsense")]
            [InlineData("10 20pt")]
            public void Returns_null_for_unparseable_value(string? value)
            {
                Assert.Null(CssParser.ParseLength(value!));
            }
        }

        public class ParseColor
        {
            [Theory]
            [InlineData("#f00")]
            [InlineData("#ff0000")]
            [InlineData("#ff0000ff")]
            [InlineData("rgb(255, 0, 0)")]
            [InlineData("rgba(255, 0, 0, 1)")]
            [InlineData("red")]
            public void Parses_supported_formats(string value)
            {
                Assert.NotNull(CssParser.ParseColor(value));
            }

            [Fact]
            public void Shorthand_hex_equals_full_hex()
            {
                Assert.Equal(
                    CssParser.ParseColor("#ff0000")!.ToString(),
                    CssParser.ParseColor("#f00")!.ToString());
            }

            [Fact]
            public void Hex_equals_equivalent_rgb()
            {
                Assert.Equal(
                    CssParser.ParseColor("#ff0000")!.ToString(),
                    CssParser.ParseColor("rgb(255, 0, 0)")!.ToString());
            }

            [Fact]
            public void Is_case_insensitive()
            {
                Assert.Equal(
                    CssParser.ParseColor("#ff0000")!.ToString(),
                    CssParser.ParseColor("#FF0000")!.ToString());
            }

            [Theory]
            [InlineData("")]
            [InlineData("   ")]
            [InlineData("definitely-not-a-color")]
            public void Returns_null_for_unknown_value(string value)
            {
                Assert.Null(CssParser.ParseColor(value));
            }
        }
    }
}
