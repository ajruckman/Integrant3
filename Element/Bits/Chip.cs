using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Chip : Bit
    {
        public Chip(BitSpec spec)
        {
            spec.ValidateFor(nameof(Chip));
            
            Spec = spec;
            
            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Chip)
            );

            if (spec.IsStatic)
            {
                Style(true);

                Class(true);
            }
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

            builder.AddContent(++seq, Spec.Content?.Invoke());
            builder.CloseElement();
        };
    }
    
    // public class Chip : IBit
    // {
    //     private readonly Content _content;
    //
    //     private readonly ClassSet _classSet;
    //     private readonly string?  _style;
    //
    //     public Chip
    //     (
    //         Content content, string? backgroundColor = null,
    //         string? textColor = null,
    //         Size?   margin    = null, Size?   padding = null,
    //         double? rem       = null, ushort? weight  = null
    //     )
    //     {
    //         _content = content;
    //         _classSet = new ClassSet(
    //             "Integrant.Element.Bit",
    //             "Integrant.Element.Bit." + nameof(Chip)
    //         );
    //
    //         _style = BitBuilder.StyleAttribute(
    //             margin: margin, padding: padding,
    //             fontSize: rem, weight: weight,
    //             backgroundColor: backgroundColor, foregroundColor: textColor
    //         );
    //     }
    //
    //     public Chip
    //     (
    //         BitGetters.BitContent?         content,                BitGetters.BitIsVisible? isVisible  = null,
    //         BitGetters.BitBackgroundColor? backgroundColor = null, BitGetters.BitColor? textColor  = null,
    //         BitGetters.BitMargin?          bitMargin       = null, BitGetters.BitPadding?   bitPadding = null,
    //         BitGetters.BitREM?             bitREM          = null, BitGetters.BitWeight?    bitWeight  = null
    //     )
    //     {
    //         Content         = content;
    //         IsVisible       = isVisible;
    //         BackgroundColor = backgroundColor;
    //         TextColor       = textColor;
    //         BitMargin       = bitMargin;
    //         BitPadding      = bitPadding;
    //         BitREM          = bitREM;
    //         BitWeight       = bitWeight;
    //     }
    //
    //     public readonly BitGetters.BitContent?         Content;
    //     public readonly BitGetters.BitIsVisible?       IsVisible;
    //     public readonly BitGetters.BitBackgroundColor? BackgroundColor;
    //     public readonly BitGetters.BitColor?       TextColor;
    //     public readonly BitGetters.BitMargin?          BitMargin;
    //     public readonly BitGetters.BitPadding?         BitPadding;
    //     public readonly BitGetters.BitREM?             BitREM;
    //     public readonly BitGetters.BitWeight?          BitWeight;
    //

    // }
}