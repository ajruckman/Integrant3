using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Arrow : IBit
    {
        private readonly ClassSet _classSet;
        private readonly string?  _style;

        public Arrow
        (
            Size?   margin          = null, Size?   padding   = null,
            double? rem             = null, ushort? weight    = null,
            string? backgroundColor = null, string? textColor = null
        )
        {
            _classSet = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Arrow)
            );

            _style = BitBuilder.StyleAttribute(
                margin: margin, padding: padding,
                rem: rem, weight: weight,
                backgroundColor: backgroundColor, textColor: textColor
            );
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classSet.Format());

            if (_style != null)
                builder.AddAttribute(++seq, "style", _style);

            builder.AddContent(++seq, "⮞");
            builder.CloseElement();
        };
    }
}