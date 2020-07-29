using System.Collections.Generic;
using Integrant.Element.Layouts;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Constructs
{
    public class Header : IConstruct
    {
        private readonly LinearLayout _layout;
        private readonly bool         _borderTop;
        private readonly bool         _borderBottom;

        public Header(List<IBit>? contents = null, bool borderTop = false, bool borderBottom = true)
        {
            _layout       = new LinearLayout(contents);
            _borderTop    = borderTop;
            _borderBottom = borderBottom;
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Construct Integrant.Element.Construct.Header");
            builder.AddContent(++seq, _layout.Render());
            builder.CloseElement();
        };
    }
}