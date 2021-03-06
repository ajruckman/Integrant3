using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Title : BitBase
    {
        public Title
        (
            BitGetters.BitContent    content,
            BitGetters.BitURL?       url             = null,
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
                URL             = url,
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

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Title)
            );

            if (url != null)
                ConstantClasses.Add("Integrant.Element.Bit." + nameof(Title) + ":Linked");

            Cache();
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            if (Spec.URL == null)
            {
                BitBuilder.OpenElement(builder, ref seq, "div", this, null, null);
            }
            else
            {
                BitBuilder.OpenElement(builder, ref seq, "a", this, null, null);

                builder.AddAttribute(++seq, "href", Spec.URL.Invoke());
            }

            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);
            
            BitBuilder.CloseElement(builder);
        };
    }
}