using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Image : Bit
    {
        private readonly BitGetters.BitPixels? _pixelsHeight;
        private readonly BitGetters.BitPixels? _pixelsWidth;

        public Image
        (
            BitGetters.BitURL        url,
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
            BitGetters.BitDisplay?   display         = null
        )
        {
            Spec = new BitSpec
            {
                URL             = url,
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                Classes         = classes,
                Margin          = margin,
                Padding         = padding,
                BackgroundColor = backgroundColor,
                ForegroundColor = foregroundColor,
                // PixelsHeight    = pixelsHeight,
                // PixelsWidth     = pixelsWidth,
                FontSize   = fontSize,
                FontWeight = fontWeight,
                Display    = display,
            };

            _pixelsHeight = pixelsHeight;
            _pixelsWidth  = pixelsWidth;

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Image)
            );
            
            Cache();
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            // Define these in HTML instead of CSS.
            uint? height = _pixelsHeight?.Invoke() ?? null;
            uint? width  = _pixelsWidth?.Invoke()  ?? null;

            builder.OpenElement(++seq, "img");
            builder.AddAttribute(++seq, "style", Style(false));
            builder.AddAttribute(++seq, "class", Class(false));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.AddAttribute(++seq, "src",    Spec.URL!.Invoke());
            builder.AddAttribute(++seq, "height", height);
            builder.AddAttribute(++seq, "width",  width);

            builder.CloseElement();
        };
    }
}