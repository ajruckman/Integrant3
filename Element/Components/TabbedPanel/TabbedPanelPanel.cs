using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Element.Components.TabbedPanel
{
    public class TabbedPanelPanel : ComponentBase
    {
        [CascadingParameter]
        public TabbedPanelContainer TabbedPanelContainer { get; set; } = null!;

        [Parameter]
        public string ID { get; set; } = null!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void OnParametersSet()
        {
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",  "Integrant.Element.Component.TabbedPanel.Panel");
            builder.AddAttribute(++seq, "hidden", TabbedPanelContainer.FocusedID() != ID);

            builder.AddContent(++seq, ChildContent);

            builder.CloseElement();
        }
    }
}