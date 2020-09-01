using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Filler : BitBase
    {
        public Filler
        (
            bool                     isStatic  = true,
            BitGetters.BitIsVisible? isVisible = null
        )
        {
            Spec = new BitSpec
            {
                IsStatic     = isStatic,
                IsVisible    = isVisible,
            };

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Filler)
            );

            Cache();
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            BitBuilder.OpenElement(builder, ref seq, "div", this, null, null);

            BitBuilder.CloseElement(builder);
        };
    }
}