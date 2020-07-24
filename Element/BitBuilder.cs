using System.Linq;

namespace Integrant.Element
{
    public static class BitBuilder
    {
        public static string? StyleAttribute
        (
            Size?    margin          = null,
            Size?    padding         = null,
            Display? display         = null,
            double?  rem             = null, ushort? weight    = 400,
            string?  backgroundColor = null, string? textColor = null
        )
        {
            // if (margin == null && rem == null) return null;

            string[] result = { };

            if (margin != null)
                result = result.Append(
                    $"margin: {margin.Value.Top}px {margin.Value.Right}px {margin.Value.Bottom}px {margin.Value.Left}px;"
                ).ToArray();

            if (padding != null)
                result = result.Append(
                    $"padding: {padding.Value.Top}px {padding.Value.Right}px {padding.Value.Bottom}px {padding.Value.Left}px;"
                ).ToArray();

            if (display != null)
                result = result.Append(
                    $@"display: {display switch
                    {
                        Display.Inline      => "inline",
                        Display.InlineBlock => "inline-block",
                        Display.Block       => "block",
                        _                   => "",
                    }};"
                ).ToArray();

            if (rem != null)
                result = result.Append($"font-size: {rem}rem;").ToArray();

            if (weight != null)
                result = result.Append($"font-weight: {weight};").ToArray();

            if (backgroundColor != null)
                result = result.Append($"background-color: {backgroundColor};").ToArray();

            if (textColor != null)
                result = result.Append($"color: {textColor};").ToArray();

            return result.Length == 0 ? null : string.Join(' ', result);
        }
    }
}