using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Element
{
    internal static class BitBuilder
    {
        internal static void OpenElement
        (
            RenderTreeBuilder builder,
            ref int           seq,
            string            element,
            BitBase           bitBase,
            string[]?         additionalStyle,
            string[]?         additionalClasses
        )
        {
            builder.OpenElement(++seq, element);
            builder.AddAttribute(++seq, "style", bitBase.Style(false, additionalStyle));
            builder.AddAttribute(++seq, "class", bitBase.Class(false, additionalClasses));

            ++seq;
            if (bitBase.Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");
        }

        internal static void CloseElement(RenderTreeBuilder builder) => builder.CloseElement();
        
        internal static string? StyleAttribute(BitSpec spec, string[]? additional = null)
        {
            List<string> result = new List<string>();

            if (spec.Margin != null)
            {
                Size v = spec.Margin.Invoke();
                result.Add($"margin: {v.Top}px {v.Right}px {v.Bottom}px {v.Left}px;");
            }

            if (spec.Padding != null)
            {
                Size v = spec.Padding.Invoke();
                result.Add($"padding: {v.Top}px {v.Right}px {v.Bottom}px {v.Left}px;");
            }

            if (spec.ForegroundColor != null)
            {
                result.Add($"color: {spec.ForegroundColor.Invoke()};");
            }

            if (spec.BackgroundColor != null)
            {
                result.Add($"background-color: {spec.BackgroundColor.Invoke()};");
            }

            if (spec.PixelsHeight != null)
            {
                result.Add($"height: {spec.PixelsHeight.Invoke()}px;");
            }

            if (spec.PixelsWidth != null)
            {
                result.Add($"width: {spec.PixelsWidth.Invoke()}px;");
            }

            if (spec.FontSize != null)
            {
                result.Add($"font-size: {spec.FontSize.Invoke()}rem;");
            }

            if (spec.FontWeight != null)
            {
                result.Add($"font-weight: {spec.FontWeight.Invoke()};");
            }

            if (spec.Display != null)
            {
                result.Add($"display: {spec.Display.Invoke()};");
            }

            if (additional != null)
                result.AddRange(additional);

            return result.Count > 0 ? string.Join(' ', result) : null;
        }
    }
}