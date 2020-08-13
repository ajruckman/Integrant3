using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Layouts
{
    public sealed class LinearLayout : ILayout
    {
        public List<IBit> Contents { get; }

        public LinearLayout(List<IBit>? contents = null)
        {
            Contents = contents ?? new List<IBit>();
        }

        public void Add(IBit bit) => Contents.Add(bit);

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout Integrant.Element.Layout.LinearLayout");
            
            foreach (var bit in Contents)
            {
                // builder.OpenElement(++seq, "section");
                // builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.LinearLayout.Child");
                builder.AddContent(++seq, bit.Render());
                // builder.CloseElement();
            }
            
            builder.CloseElement();
        };
    }
}