using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Layouts
{
    public class DropdownLayout : ILayout
    {
        public IBit       Top      { get; }
        public List<IBit> Contents { get; }

        public DropdownLayout(IBit top, List<IBit>? contents = null)
        {
            Top      = top;
            Contents = contents ?? new List<IBit>();
        }

        public void Add(IBit bit) => Contents.Add(bit);

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout Integrant.Element.Layout.DropdownLayout");

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.DropdownLayout.Top");
            builder.AddContent(++seq, Top.Render());
            builder.CloseElement();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.DropdownLayout.Contents");

            for (var i = 0; i < Contents.Count; i++)
            {
                IBit? bit = Contents[i];
                builder.AddContent(++seq, bit.Render());

                if (i < Contents.Count - 1)
                {
                    builder.OpenElement(++seq, "div");
                    builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.DropdownLayout.Separator");
                    builder.CloseElement();
                }
            }

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}