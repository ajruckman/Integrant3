using Integrant.Colorant.Themes.Default;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Spinner : BitBase
    {
        private readonly BitGetters.BitPixels? _pixelsSize;
        private readonly BitGetters.BitColor?  _color;
        private readonly BitGetters.BitPixels? _pixelsThickness;

        public Spinner
        (
            BitGetters.BitPixels?    pixelsSize      = null,
            BitGetters.BitColor?     color           = null,
            BitGetters.BitPixels?    pixelsThickness = null,
            bool                     isStatic        = true,
            BitGetters.BitIsVisible? isVisible       = null,
            BitGetters.BitDisplay?   display         = null
        )
        {
            Spec = new BitSpec
            {
                IsStatic  = isStatic,
                IsVisible = isVisible,
                Display   = display,
            };

            _pixelsSize      = pixelsSize;
            _color           = color;
            _pixelsThickness = pixelsThickness;

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
            double size      = _pixelsSize?.Invoke()      ?? 64;
            string color     = _color?.Invoke()           ?? Constants.Accent_4;
            uint   thickness = _pixelsThickness?.Invoke() ?? 4;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "style", Style(false));
            builder.AddAttribute(++seq, "class", Class(false));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.OpenElement(++seq, "svg");

            builder.AddAttribute(++seq, "height",  size);
            builder.AddAttribute(++seq, "width",   size);
            builder.AddAttribute(++seq, "viewBox", $"0 0 50 50");

            //

            builder.AddMarkupContent(++seq,
                $"<circle "                    +
                $"cx='25' "                    +
                $"cy='25' "                    +
                $"r='20' "                     +
                $"fill='none' "                +
                $"stroke-dasharray='1, 200' "  +
                $"stroke-dashoffset='0' "      +
                $"stroke-linecap='round' "     +
                $"stroke-width='{thickness}' " +
                $"stroke='{color}'"            +
                $"/>");

            //

            builder.CloseElement();
            builder.CloseElement();
        };
    }
}