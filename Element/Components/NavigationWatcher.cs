using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace Integrant.Element.Components
{
    public class NavigationWatcher : ComponentBase, IDisposable
    {
        [Parameter] public NavigationManager NavigationManager { get; set; }
        [Parameter] public RenderFragment    ChildContent      { get; set; }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= Render;
            GC.SuppressFinalize(this);
        }

        protected override void OnParametersSet()
        {
            NavigationManager.LocationChanged += Render;
        }

        protected override void BuildRenderTree(RenderTreeBuilder b)
        {
            b.AddContent(0, ChildContent);
        }

        private void Render(object? sender, LocationChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}