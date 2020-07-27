using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Arrow : Bit
    {
        public Arrow(BitSpec spec)
        {
            spec.ValidateFor(nameof(Arrow));
            
            Spec = spec;
            
            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Arrow)
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

            builder.AddContent(++seq, "⮞");
            builder.CloseElement();
        };
    }
}