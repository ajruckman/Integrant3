using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Chip : IBit
    {
        private readonly Content _content;

        private readonly ClassSet _classSet;
        private readonly string?  _style;

        public Chip
        (
            Content content,
            string? backgroundColor = null, string? textColor = null,
            Size?   margin          = null, Size?   padding   = null,
            double? rem             = null, ushort? weight    = null
        )
        {
            _content = content;
            _classSet = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Chip)
            );

            _style = BitBuilder.StyleAttribute(
                margin: margin, padding: padding,
                rem: rem, weight: weight,
                backgroundColor: backgroundColor, textColor: textColor
            );
        }

        public Chip
        (
            BitGetters.BitIsVisible? isVisible  = null, BitGetters.BitBackgroundColor? backgroundColor = null,
            BitGetters.BitTextColor? textColor  = null, BitGetters.BitMargin?          bitMargin       = null,
            BitGetters.BitPadding?   bitPadding = null, BitGetters.BitREM?             bitRem          = null,
            BitGetters.BitWeight?    bitWeight  = null
        )
        {
            IsVisible       = isVisible;
            BackgroundColor = backgroundColor;
            TextColor       = textColor;
            BitMargin       = bitMargin;
            BitPadding      = bitPadding;
            BitREM          = bitRem;
            BitWeight       = bitWeight;
        }

        public readonly BitGetters.BitIsVisible?       IsVisible;
        public readonly BitGetters.BitBackgroundColor? BackgroundColor;
        public readonly BitGetters.BitTextColor?       TextColor;
        public readonly BitGetters.BitMargin?          BitMargin;
        public readonly BitGetters.BitPadding?         BitPadding;
        public readonly BitGetters.BitREM?             BitREM;
        public readonly BitGetters.BitWeight?          BitWeight;

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classSet.Format());

            if (_style != null)
                builder.AddAttribute(++seq, "style", _style);

            builder.AddContent(++seq, _content.Value);
            builder.CloseElement();
        };
    }
}