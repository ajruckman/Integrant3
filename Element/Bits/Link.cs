using System.Linq;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Link : Bit
    {
        public Link
        (
            BitGetters.BitContent        content,
            BitGetters.BitURL            url,
            bool                         isStatic        = true,
            BitGetters.BitIsVisible?     isVisible       = null,
            BitGetters.BitClasses?       classes         = null,
            BitGetters.BitSize?          margin          = null,
            BitGetters.BitSize?          padding         = null,
            BitGetters.BitColor?         foregroundColor = null,
            BitGetters.BitColor?         backgroundColor = null,
            BitGetters.BitPixels?        pixelsHeight    = null,
            BitGetters.BitPixels?        pixelsWidth     = null,
            BitGetters.BitREM?           fontSize        = null,
            BitGetters.BitWeight?        fontWeight      = null,
            BitGetters.BitDisplay?       display         = null,
            BitGetters.BitIsHighlighted? isHighlighted   = null
        )
        {
            Spec = new BitSpec
            {
                Content         = content,
                URL             = url,
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                Classes         = classes,
                Margin          = margin,
                Padding         = padding,
                ForegroundColor = foregroundColor,
                BackgroundColor = backgroundColor,
                PixelsHeight    = pixelsHeight,
                PixelsWidth     = pixelsWidth,
                FontSize        = fontSize,
                FontWeight      = fontWeight,
                Display         = display,
                IsHighlighted   = isHighlighted,
            };

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Link)
            );

            Cache(additionalClasses: LocalClasses());
        }

        private string[]? LocalClasses()
        {
            return Spec.IsHighlighted?.Invoke() == true
                ? new[] {"Integrant.Element.Bit." + nameof(Link) + ":Highlighted"}
                : null;
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "a");
            builder.AddAttribute(++seq, "style", Style(false));
            builder.AddAttribute(++seq, "class", Class(false, LocalClasses()));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.AddAttribute(++seq, "href", Spec.URL!.Invoke());
            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);
            builder.CloseElement();
        };
    }
}