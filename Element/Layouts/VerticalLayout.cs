using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Layouts
{
    public class VerticalLayout : ILayout
    {
        // private readonly bool _left;

        // public IBit       Top      { get; }
        public List<IBit> Contents { get; }

        public VerticalLayout(List<IBit>? contents = null /*, bool left = false*/)
        {
            // _left    = left;
            // Top      = top;
            Contents = contents ?? new List<IBit>();
        }

        public void Add(IBit bit) => Contents.Add(bit);

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout Integrant.Element.Layout.VerticalLayout");

            foreach (var bit in Contents)
            {
                // builder.OpenElement(++seq, "section");
                // builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.VerticalLayout.Child");
                builder.AddContent(++seq, bit.Render());
                // builder.CloseElement();
            }

            builder.CloseElement();

            // int seq = -1;
            //
            // builder.OpenElement(++seq, "div");
            // builder.AddAttribute(++seq, "class",
            //     "Integrant.Element.Layout Integrant.Element.Layout.VerticalLayout" + (_left
            //         ? " Integrant.Element.Layout.VerticalLayout:Left"
            //         : ""));
            //
            // builder.OpenElement(++seq, "div");
            // builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.VerticalLayout.Top");
            // builder.AddContent(++seq, Top.Render());
            // builder.CloseElement();
            //
            // builder.OpenElement(++seq, "div");
            // builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.VerticalLayout.Contents");
            //
            // for (var i = 0; i < Contents.Count; i++)
            // {
            //     builder.OpenElement(++seq, "div");
            //     builder.AddAttribute(++seq, "class", "Integrant.Element.Layout.VerticalLayout.Child");
            //
            //     IBit? bit = Contents[i];
            //     builder.AddContent(++seq, bit.Render());
            //
            //     builder.CloseElement();
            // }
            //
            // builder.CloseElement();
            //
            // builder.CloseElement();
        };
    }
}