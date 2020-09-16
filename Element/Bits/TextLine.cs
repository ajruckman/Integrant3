using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class TextLine : BitBase
    {
        public TextLine
        (
            BitGetters.BitContent    content,
            bool                     isStatic        = true,
            BitGetters.BitIsVisible? isVisible       = null,
            BitGetters.BitClasses?   classes         = null,
            BitGetters.BitSize?      margin          = null,
            BitGetters.BitSize?      padding         = null,
            BitGetters.BitColor?     backgroundColor = null,
            BitGetters.BitColor?     foregroundColor = null,
            BitGetters.BitPixels?    pixelsHeight    = null,
            BitGetters.BitPixels?    pixelsWidth     = null,
            BitGetters.BitREM?       fontSize        = null,
            BitGetters.BitWeight?    fontWeight      = null,
            BitGetters.BitDisplay?   display         = null,
            BitGetters.BitData?      data            = null
        )
        {
            Spec = new BitSpec
            {
                Content         = content,
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                Classes         = classes,
                Margin          = margin,
                Padding         = padding,
                BackgroundColor = backgroundColor,
                ForegroundColor = foregroundColor,
                PixelsHeight    = pixelsHeight,
                PixelsWidth     = pixelsWidth,
                FontSize        = fontSize,
                FontWeight      = fontWeight,
                Display         = display,
                Data            = data,
            };

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(TextLine)
            );

            Cache();
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            BitBuilder.OpenElement(builder, ref seq, "div", this, null, null);

            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);
            
            BitBuilder.CloseElement(builder);
        };
    }
}