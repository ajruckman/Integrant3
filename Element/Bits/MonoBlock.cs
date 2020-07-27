using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    // public class MonoBlock : IBit
    // {
    //     private readonly Content _content;
    //
    //     private readonly ClassSet _classSet;
    //     private readonly string?  _style;
    //
    //     public MonoBlock
    //     (
    //         Content content,
    //         string? backgroundColor = null, string? textColor = null,
    //         Size?   margin          = null, Size?   padding   = null,
    //         double? rem             = null, ushort? weight    = null
    //     )
    //     {
    //         _content = content;
    //         _classSet = new ClassSet(
    //             "Integrant.Element.Bit",
    //             "Integrant.Element.Bit." + nameof(MonoBlock)
    //         );
    //
    //         _style = BitBuilder.StyleAttribute(
    //             margin: margin, padding: padding,
    //             fontSize: rem, weight: weight,
    //             backgroundColor: backgroundColor, foregroundColor: textColor
    //         );
    //     }
    //
    //     public RenderFragment Render() => builder =>
    //     {
    //         int seq = -1;
    //
    //         builder.OpenElement(++seq, "div");
    //         builder.AddAttribute(++seq, "class", _classSet.Format());
    //
    //         if (_style != null)
    //             builder.AddAttribute(++seq, "style", _style);
    //
    //         builder.AddContent(++seq, _content.Value);
    //         builder.CloseElement();
    //     };
    // }
}