using System;
using System.Collections.Generic;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Layouts
{
    public class VerticalLayout : ILayout
    {
        private bool _hasRendered;

        public VerticalLayout(List<IBit>? contents = null)
        {
            Contents = contents ?? new List<IBit>();
        }

        public List<IBit> Contents { get; }

        public void Add(IBit bit)
        {
            if (_hasRendered)
                throw new Exception("This Layout has already been rendered and new Bits cannot be added to it.");

            Contents.Add(bit);
        }

        public RenderFragment Render() => builder =>
        {
            _hasRendered = true;

            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Layout Integrant.Element.Layout.VerticalLayout");

            foreach (var bit in Contents)
            {
                builder.AddContent(++seq, bit.Render());
            }

            builder.CloseElement();
        };
    }
}