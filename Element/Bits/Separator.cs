using System.Linq;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Separator : BitBase
    {
        private readonly BitGetters.BitPixels? _pixelsThickness;
        private readonly BitGetters.BitColor?  _color;

        public Separator
        (
            bool                     isStatic        = true,
            BitGetters.BitPixels?    pixelsThickness = null,
            BitGetters.BitColor?     color           = null,
            BitGetters.BitIsVisible? isVisible       = null,
            BitGetters.BitClasses?   classes         = null,
            BitGetters.BitSize?      margin          = null,
            BitGetters.BitSize?      padding         = null,
            BitGetters.BitPixels?    pixelsHeight    = null,
            BitGetters.BitDisplay?   display         = null

            // byte pxHeight = 16, byte pxWidth = 5, byte pxThickness = 1, string color = Colors.HeaderVerticalLine
        )
        {
            _pixelsThickness = pixelsThickness;
            _color           = color;

            Spec = new BitSpec
            {
                IsStatic     = isStatic,
                IsVisible    = isVisible,
                Classes      = classes,
                Margin       = margin,
                Padding      = padding,
                PixelsHeight = pixelsHeight,
                Display      = display,
            };

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Separator)
            );

            Cache(LocalStyle());
        }

        private string[]? LocalStyle()
        {
            string[] style = { };

            if (_color != null)
                style = style.Append($"border-left-color: {_color.Invoke()}").ToArray();

            if (_pixelsThickness != null)
                style = style.Append($"border-left-width: {_pixelsThickness.Invoke()}px").ToArray();

            return style.Length > 0 ? style : null;
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "style", Style(false, LocalStyle()));
            builder.AddAttribute(++seq, "class", Class(false));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.CloseElement();
        };
    }
}