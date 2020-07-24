using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Heading : IBit
    {
        private readonly Content _content;

        private readonly string   _element;
        private readonly ClassSet _classSet;
        private readonly string?  _style;

        public enum Size
        {
            H1 = 1, H2 = 2, H3 = 3, H4 = 4, H5 = 5, H6 = 6,
        }

        public Heading
        (
            Content       content, Size size = Size.H1,
            Display?      display         = null,
            Element.Size? margin          = null, Element.Size? padding   = null,
            double?       rem             = null, ushort?       weight    = null,
            string?       backgroundColor = null, string?       textColor = null
        )
        {
            _content = content;
            _element = ("h" + (int) size);

            _classSet = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Heading)
            );

            _style = BitBuilder.StyleAttribute(
                margin: margin, padding: padding, display: display,
                rem: rem, weight: weight,
                backgroundColor: backgroundColor, textColor: textColor
            );
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, _element);
            builder.AddAttribute(++seq, "class", _classSet.Format());

            if (_style != null)
                builder.AddAttribute(++seq, "style", _style);

            builder.AddContent(++seq, _content.Value);
            builder.CloseElement();
        };
    }
}