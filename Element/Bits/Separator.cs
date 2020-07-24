using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Separator : IBit
    {
        private readonly byte   _pxHeight;
        private readonly byte   _pxWidth;
        private readonly byte   _pxThickness;
        private readonly string _color;

        private readonly ClassSet _classSet;

        public Separator
        (
            byte pxHeight = 16, byte pxWidth = 5, byte pxThickness = 1, string color = Colors.HeaderVerticalLine
        )
        {
            _pxHeight    = pxHeight;
            _pxWidth     = pxWidth;
            _pxThickness = pxThickness;
            _color       = color;
            _classSet = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Separator)
            );
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classSet.Format());
            builder.AddAttribute(++seq, "style",
                $"height: {_pxHeight}px; margin: 0 {_pxWidth}px; border-left: {_pxThickness}px solid {_color};");
            builder.CloseElement();
        };
    }
}