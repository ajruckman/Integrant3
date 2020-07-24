using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Image : IBit
    {
        private readonly string _url;
        private readonly byte?  _pxHeight;
        private readonly byte?  _pxWidth;

        private readonly ClassSet _classSet;
        private readonly string?  _style;

        public Image
        (
            string  url,
            Size?   margin          = null, Size? padding = null,
            byte?   pxHeight        = null, byte? pxWidth = null,
            string? backgroundColor = null
        )

        {
            _url      = url;
            _pxHeight = pxHeight;
            _pxWidth  = pxWidth;
            _classSet = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Image)
            );

            _style = BitBuilder.StyleAttribute(margin, padding, backgroundColor: backgroundColor);
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "img");
            builder.AddAttribute(++seq, "src",    _url);
            builder.AddAttribute(++seq, "height", _pxHeight);
            builder.AddAttribute(++seq, "width",  _pxWidth);

            builder.AddAttribute(++seq, "class", _classSet.Format());

            if (_style != null)
                builder.AddAttribute(++seq, "style", _style);

            builder.CloseElement();
        };
    }
}