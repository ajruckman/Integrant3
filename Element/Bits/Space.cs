using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Space : BitBase
    {
        public Space
        (
            BitGetters.BitPixels?    pixelsWidth = null,
            bool                     isStatic    = true,
            BitGetters.BitIsVisible? isVisible   = null,
            BitGetters.BitDisplay?   display     = null,
            BitGetters.BitData?      data        = null
        )
        {
            Spec = new BitSpec
            {
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                PixelsWidth     = pixelsWidth,
                Display         = display,
                Data            = data,
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

            BitBuilder.OpenElement(builder, ref seq, "div", this, null, null);

            BitBuilder.CloseElement(builder);
        };
    }
}