using Integrant.Colorant.Themes.Default;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Spinner : BitBase
    {
        private readonly BitGetters.BitPixels? _pixelsSize;
        private readonly BitGetters.BitColor?  _color;
        private readonly BitGetters.BitPixels? _thickness;

        public Spinner
        (
            BitGetters.BitPixels?    pixelsSize = null,
            BitGetters.BitColor?     color      = null,
            BitGetters.BitPixels?    thickness  = null,
            bool                     isStatic   = true,
            BitGetters.BitIsVisible? isVisible  = null,
            BitGetters.BitDisplay?   display    = null,
            BitGetters.BitData?      data       = null,
            BitGetters.BitTooltip?   tooltip    = null
        )
        {
            Spec = new BitSpec
            {
                IsStatic  = isStatic,
                IsVisible = isVisible,
                Display   = display,
                Data      = data,
                Tooltip   = tooltip,
            };

            _pixelsSize = pixelsSize;
            _color      = color;
            _thickness  = thickness;

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Spinner)
            );

            Cache();
        }

        // From:
        // https://codepen.io/jczimm/pen/vEBpoL

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            // Define this in HTML instead of CSS.
            double size      = _pixelsSize?.Invoke() ?? 64;
            string color     = _color?.Invoke()      ?? Constants.Accent_4;
            double thickness = _thickness?.Invoke()  ?? 4;

            BitBuilder.OpenElement(builder, ref seq, "div", this, null, null);

            builder.OpenElement(++seq, "svg");

            builder.AddAttribute(++seq, "height",  size);
            builder.AddAttribute(++seq, "width",   size);
            builder.AddAttribute(++seq, "viewBox", $"0 0 100 100");

            //

            builder.AddMarkupContent(++seq,
                $"<circle "     +
                $"cx='50' "     +
                $"cy='50' "     +
                $"r='40' "      +
                $"fill='none' " +
                // $"stroke-dasharray='200, 400' "  +
                $"stroke-dashoffset='0' "      +
                $"stroke-linecap='round' "     +
                $"stroke-width='{thickness}' " +
                $"stroke='{color}'"            +
                $"/>");

            //

            builder.CloseElement();

            BitBuilder.CloseElement(builder);
        };
    }
}