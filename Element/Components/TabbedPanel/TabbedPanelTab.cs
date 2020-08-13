using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Integrant.Element.Components.TabbedPanel
{
    public class TabbedPanelTab : ComponentBase
    {
        [CascadingParameter]
        public TabbedPanelContainer TabbedPanelContainer { get; set; } = null!;

        [Parameter]
        public string ID { get; set; } = null!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void OnParametersSet()
        {
            TabbedPanelContainer.Register(ID);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Component.TabbedPanel.Tab" +
                                                 (TabbedPanelContainer.FocusedID() == ID
                                                     ? " Integrant.Element.Component.TabbedPanel.Tab:Focused"
                                                     : ""));
            @builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClick));

            builder.AddContent(++seq, ChildContent);

            builder.CloseElement();
        }

        private void OnClick(MouseEventArgs args)
        {
            TabbedPanelContainer.Focus(ID);
        }
    }
}