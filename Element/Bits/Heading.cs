using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Heading : BitBase
    {
        public enum Size
        {
            H1 = 1, H2 = 2, H3 = 3, H4 = 4, H5 = 5, H6 = 6,
        }

        private readonly string _element;

        public Heading
        (
            BitGetters.BitContent    content,
            Size                     size            = Size.H1,
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
            BitGetters.BitData?      data            = null,
            BitGetters.BitTooltip?   tooltip         = null
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
                Tooltip         = tooltip,
            };

            _element = ("h" + (int) size);

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Heading)
            );

            Cache();
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;
            
            BitBuilder.OpenElement(builder, ref seq, _element, this, null, null);
            
            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);

            BitBuilder.CloseElement(builder);
        };
    }
}