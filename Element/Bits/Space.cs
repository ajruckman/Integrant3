using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Space : BitBase
    {
        public Space
        (
            BitGetters.BitPixels?    pixelsWidth     = null,
            bool                     isStatic        = true,
            BitGetters.BitIsVisible? isVisible       = null,
            BitGetters.BitDisplay?   display         = null
        )
        {
            Spec = new BitSpec
            {
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                PixelsWidth     = pixelsWidth,
                Display         = display,
            };

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Space)
            );
            
            Cache();
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "style", Style(false));
            builder.AddAttribute(++seq, "class", Class(false));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.CloseElement();
        };
    }
}