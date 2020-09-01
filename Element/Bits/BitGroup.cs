using System.Collections.Generic;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    /// <summary>
    ///     A container Bit which may be used to use single Getters for multiple
    ///     child Bits.
    /// </summary>
    public class BitGroup : IBit
    {
        private readonly List<IBit>               _contents;
        private readonly BitGetters.BitIsVisible? _isVisible;

        public BitGroup
        (
            List<IBit>               contents,
            BitGetters.BitIsVisible? isVisible = null
        )
        {
            _contents  = contents;
            _isVisible = isVisible;
        }

        public RenderFragment Render() => b =>
        {
            int seq = -1;

            b.OpenElement(++seq, "section");
            b.AddAttribute(++seq, "class",  "Integrant.Element.Bit Integrant.Element.Bit.BitGroup");
            b.AddAttribute(++seq, "hidden", _isVisible?.Invoke() == false);

            foreach (IBit bit in _contents)
            {
                b.AddContent(++seq, bit.Render());
            }

            b.CloseElement();
        };
    }
}