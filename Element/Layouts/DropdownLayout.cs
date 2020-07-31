using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Layouts
{
    public class DropdownLayout : ILayout
    {
        private readonly bool _left;

        public IBit       Top      { get; }
        public List<IBit> Contents { get; }

        public DropdownLayout(IBit top, List<IBit>? contents = null, bool left = false)
        {
            _left    = left;
            Top      = top;
            Contents = contents ?? new List<IBit>();
        }

        public void Add(IBit bit) => Contents.Add(bit);

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",
                "Integrant.Element.Layout Integrant.Element.Layout.DropdownLayout" + (_left
                    ? " Integrant.Element.Layout.DropdownLayout:Left"
                    : ""));

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.DropdownLayout.Top");
            builder.AddContent(++seq, Top.Render());
            builder.CloseElement();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.DropdownLayout.Contents");

            for (var i = 0; i < Contents.Count; i++)
            {
                builder.OpenElement(++seq, "div");
                builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.DropdownLayout.Child");

                IBit? bit = Contents[i];
                builder.AddContent(++seq, bit.Render());

                builder.CloseElement();
            }

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}