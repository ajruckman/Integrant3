using System;
using Integrant.Resources.Icons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Superset.Web.Markup;

namespace Integrant.Element.Components.Modal
{
    public sealed class ModalContent : ComponentBase, IDisposable
    {
        private ElementReference _elemRef;

        [Parameter] public Modal?         Modal        { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;

        [InjectAttribute] public IJSRuntime JSRuntime { get; set; } = null!;

        protected override void OnParametersSet()
        {
            if (Modal == null)
                throw new ArgumentNullException(nameof(Modal), "No Modal was passed to ModalContent component.");
            Modal.StateHasChanged =  StateHasChanged;
            Modal.OnShow          += Focus;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            var rootClasses =
                new ClassSet("Integrant.Element.Component", "Integrant.Element.Component.Modal");
            if (Modal!.Shown)
                rootClasses.Add("Integrant.Element.Component.Modal:Shown");

            // -> Container

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "hidden",   !Modal!.Shown);
            builder.AddAttribute(++seq, "class",    rootClasses.ToString());
            builder.AddAttribute(++seq, "tabindex", 0);
            builder.AddAttribute(++seq, "onclick",
                EventCallback.Factory.Create<MouseEventArgs>(this, OnBackgroundClick));
            builder.AddAttribute(++seq, "onkeyup",
                EventCallback.Factory.Create<KeyboardEventArgs>(this, OnBackgroundKeyUp));
            builder.AddElementReferenceCapture(++seq, elemRef => _elemRef = elemRef);

            // - -> Constrainer

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Component.Modal.Constrainer");
            builder.AddAttribute(++seq, "style", $"max-width: {Modal.MaxWidth()};");

            // - - -> Content container

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",   "Integrant.Element.Component.Modal.Content");
            builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnContentClick));
            builder.AddEventStopPropagationAttribute(++seq, "onclick", true);

            // - - - -> Close button

            builder.OpenElement(++seq, "span");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Component.Modal.CloseButton");
            builder.AddAttribute(++seq, "onclick",
                EventCallback.Factory.Create<MouseEventArgs>(this, OnCloseButtonClick));
            builder.OpenComponent<MaterialIcon>(++seq);
            builder.AddAttribute(++seq, "ID", "close");
            builder.CloseComponent();
            builder.CloseElement();

            // - - - -> Content

            builder.AddContent(++seq, ChildContent);

            //

            builder.CloseElement();

            builder.CloseElement();

            builder.CloseElement();
        }

        private void Focus()
        {
            JSRuntime.InvokeVoidAsync("Integrant.Element.FocusModal", _elemRef);
        }

        private void OnBackgroundKeyUp(KeyboardEventArgs args)
        {
            if (args.Key == "Escape")
            {
                Modal!.Hide();
            }
        }

        private void OnBackgroundClick(MouseEventArgs args)
        {
            Modal!.Hide();
        }

        // This is required for 'onclick:preventDefault' to work.
        private void OnContentClick(MouseEventArgs args) { }

        private void OnCloseButtonClick()
        {
            Modal!.Hide();
        }

        public void Dispose()
        {
            if (Modal != null) Modal.OnShow -= Focus;
        }
    }
}