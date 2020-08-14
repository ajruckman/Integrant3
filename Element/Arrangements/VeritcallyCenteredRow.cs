using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Element.Arrangements
{
    public class VerticallyCenteredRow : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;
            
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Arrangement.VerticallyCenteredRow");
            builder.AddContent(++seq, ChildContent);
            builder.CloseElement();
        }
    }
}