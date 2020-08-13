using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Element.Components.TabbedPanel
{
    public class TabbedPanelContainer : ComponentBase
    {
        private readonly List<string> _ids = new List<string>();
        private          string?      _focusedID;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",
                "Integrant.Element.Component Integrant.Element.Component.TabbedPanel.Container");

            builder.OpenComponent<CascadingValue<TabbedPanelContainer>>(++seq);
            builder.AddAttribute(++seq, "Value",        this);
            builder.AddAttribute(++seq, "IsFixed",      true);
            builder.AddAttribute(++seq, "ChildContent", ChildContent);
            builder.CloseComponent();

            builder.CloseElement();
        }

        internal void Register(string id)
        {
            _ids.Add(id);
            _focusedID ??= id;
        }

        internal string FocusedID() => _focusedID!;

        public void Focus(string id)
        {
            _focusedID = id;
            StateHasChanged();
        }
    }
}