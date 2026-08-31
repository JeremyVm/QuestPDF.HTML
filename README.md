# QuestPDF.HTML

QuestPDF.HTML is an extension for [QuestPDF](https://github.com/QuestPDF/QuestPDF) that allows you to generate PDF from HTML.

QuestPDF currently does not support inserting HTML into a PDF document. This library fills that gap. While it doesn't support the full functionality of HTML and CSS, it covers most common use cases.

## Dependencies

- [QuestPDF](https://github.com/QuestPDF/QuestPDF)
- [HtmlAgilityPack](https://html-agility-pack.net/) - used for HTML parsing

## Installation

```
dotnet add package QuestPDF.HTML
```

## Usage

### Basic Example

```csharp
Document.Create(container =>
{
    container.Page(page =>
    {
        page.Content().Column(col =>
        {
            col.Item().HTML(handler =>
            {
                handler.SetHtml(html);
            });
        });
    });
}).GeneratePdf(path);
```

### Custom Image Loading

**Recommended:** Override the default image loading method for better performance and async support:

```csharp
col.Item().HTML(handler =>
{
    handler.OverloadImgReceivingFunc(GetImgBySrc);
    handler.SetHtml(html);
});
```

### Custom Styles

You can customize text and container styles for specific HTML tags:

```csharp
handler.SetTextStyleForHtmlElement("div", TextStyle.Default.FontColor(Colors.Grey.Medium));
handler.SetTextStyleForHtmlElement("h1", TextStyle.Default.FontColor(Colors.DeepOrange.Accent4).FontSize(32).Bold());
handler.SetContainerStyleForHtmlElement("table", c => c.Background(Colors.Pink.Lighten5));
handler.SetContainerStyleForHtmlElement("ul", c => c.PaddingVertical(10));
```

### List Padding

Set vertical padding for top-level lists (does not apply to nested lists):

```csharp
handler.SetListVerticalPadding(40);
```

## Supported HTML Elements

### Block Elements

| Tag | Description |
|-----|-------------|
| `div` | Generic container |
| `p` | Paragraph |
| `h1` - `h6` | Headings (with default font sizes) |
| `ul`, `ol`, `li` | Lists (unordered, ordered, list items) |
| `table`, `thead`, `tbody`, `tr`, `th`, `td` | Tables (with colspan/rowspan support) |
| `blockquote` | Block quotation with left border |
| `pre` | Preformatted text (preserves whitespace, monospace font) |
| `hr` | Horizontal rule |
| `section`, `header`, `footer` | Semantic containers |

### Inline Elements

| Tag | Description |
|-----|-------------|
| `a` | Hyperlinks |
| `b`, `strong` | Bold text |
| `i`, `em` | Italic text |
| `u` | Underlined text |
| `s`, `strike`, `del` | Strikethrough text |
| `sub`, `sup` | Subscript/superscript |
| `small` | Smaller text |
| `br` | Line break |
| `img` | Images (supports URLs and base64) |
| `span` | Inline container |
| `code` | Inline code (monospace with background) |
| `kbd` | Keyboard input |
| `mark` | Highlighted text |
| `abbr` | Abbreviation |
| `q` | Inline quotation |
| `var` | Variable |
| `samp` | Sample output |

## CSS Inline Style Support

The library supports parsing inline `style` attributes on HTML elements.

### Supported Text Properties

- `color` - Text color
- `background-color` - Text background
- `font-size` - Font size (px, pt, em, rem, cm, mm, in)
- `font-weight` - Bold, normal, light, or numeric (100-900)
- `font-style` - Italic, normal
- `font-family` - Font family name
- `text-decoration` - Underline, line-through
- `letter-spacing` - Letter spacing
- `line-height` - Line height

### Supported Container Properties

- `padding`, `padding-top`, `padding-bottom`, `padding-left`, `padding-right`
- `margin` (converted to padding)
- `background-color`
- `border`, `border-color`
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `text-align` - Left, center, right

### Supported Color Formats

- Hex: `#RGB`, `#RRGGBB`, `#RRGGBBAA`
- RGB: `rgb(r, g, b)`, `rgba(r, g, b, a)`
- Named colors: `red`, `blue`, `green`, etc.

### Example

```html
<p style="color: #e74c3c; font-size: 18px; font-weight: bold;">
    Styled text with CSS
</p>
<div style="background-color: #f0f0f0; padding: 10px; border: 1px;">
    Container with background and padding
</div>
```

## Example Application

You can use [HTMLToQPDF.Example](https://github.com/JeremyVm/QuestPDF.HTML) to try out the capabilities of this extension.

<p align="center">
  <img src="https://user-images.githubusercontent.com/26045342/195960914-1aef2f7e-f5bb-4c4b-bbe9-cd4770a0527f.png" />
</p>

<table border="0">
 <tr>
    <td><b style="font-size:30px">Default Styles</b></td>
    <td><b style="font-size:30px">Options for changing styles</b></td>
 </tr>
 <tr>
    <td><img src="https://user-images.githubusercontent.com/26045342/195960950-8bf101e9-c64e-482c-9993-39f9646d0e2f.png" /></td>
    <td><img src="https://user-images.githubusercontent.com/26045342/195960936-6f014456-a074-4672-aa39-03cdcdcc3afc.png" /></td>
 </tr>
</table>

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history and release notes.

## License

MIT License - see the [LICENSE](LICENSE) file for details.

## Credits

Originally created by [Relorer](https://github.com/Relorer/HTMLToQPDF).
