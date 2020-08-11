using System.Collections.Generic;
using Integrant.Element.Bits;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Element.Components
{
    public class CenteredSpinner : ComponentBase
    {
        private Spinner _spinner = null!;

        [Parameter]                                public uint                       Size       { get; set; } = 64;
        [Parameter]                                public uint                       Thickness  { get; set; } = 4;
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; }

        protected override void OnParametersSet()
        {
            _spinner = new Spinner(() => Size, thickness: () => Thickness);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",
                "Integrant.Element.Component Integrant.Element.Component." + nameof(CenteredSpinner));
            builder.AddMultipleAttributes(++seq, Attributes);
            builder.AddContent(++seq, _spinner.Render());
            builder.CloseElement();
        }
    }
}