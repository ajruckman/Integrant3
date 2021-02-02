using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Integrant.Dominant
{
    public class HTMLTextInput : IHTMLInput<string?>
    {
        private readonly IJSRuntime _jsRuntime;

        private ElementReference _reference;

        public RenderFragment Render()
        {
            void Fragment(RenderTreeBuilder builder)
            {
                var seq = -1;

                builder.OpenElement(++seq, "input");
                builder.AddAttribute(++seq, "type", "text");
                builder.AddAttribute(++seq, "oninput", EventCallback.Factory.Create(this, Change));

                builder.AddElementReferenceCapture(++seq, r => _reference = r);
                builder.CloseElement();
            }

            return Fragment;
        }

        public HTMLTextInput(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string?> GetValue()
        {
            return await _jsRuntime.InvokeAsync<string>("window.Integrant.Dominant.GetValue", _reference);
        }

        public async Task SetValue(string? value)
        {
            await _jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetValue", _reference, value ?? "");
        }

        public event Action<string?>? OnChange;

        private void Change(ChangeEventArgs args)
        {
            OnChange?.Invoke(args.Value?.ToString());
        }
    }
}