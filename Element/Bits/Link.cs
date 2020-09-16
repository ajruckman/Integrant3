using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Link : BitBase
    {
        // public event Action? OnClick;
        
        public Link
        (
            BitGetters.BitContent        content,
            BitGetters.BitURL            url,
            bool                         isStatic        = true,
            BitGetters.BitIsVisible?     isVisible       = null,
            BitGetters.BitClasses?       classes         = null,
            BitGetters.BitSize?          margin          = null,
            BitGetters.BitSize?          padding         = null,
            BitGetters.BitColor?         backgroundColor = null,
            BitGetters.BitColor?         foregroundColor = null,
            BitGetters.BitPixels?        pixelsHeight    = null,
            BitGetters.BitPixels?        pixelsWidth     = null,
            BitGetters.BitREM?           fontSize        = null,
            BitGetters.BitWeight?        fontWeight      = null,
            BitGetters.BitDisplay?       display         = null,
            BitGetters.BitIsHighlighted? isHighlighted   = null,
            BitGetters.BitData?          data            = null,
            BitGetters.BitTooltip?       tooltip         = null
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
                IsHighlighted   = isHighlighted,
                Data            = data,
                Tooltip         = tooltip,
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

            BitBuilder.OpenElement(builder, ref seq, "a", this, null, LocalClasses());

            builder.AddAttribute(++seq, "href", Spec.URL!.Invoke());
            // builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, HandleClick));
            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);
            
            BitBuilder.CloseElement(builder);
        };

        // private void HandleClick()
        // {
        //     OnClick?.Invoke();
        // }
    }
}