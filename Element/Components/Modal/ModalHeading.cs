using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Element.Components.Modal
{
    public sealed class ModalHeading : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Component.Modal.Heading");
            builder.AddContent(++seq, ChildContent);
            builder.CloseElement();
        }
    }
}