using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Integrant.Dominant
{
    public class HTMLDateInput : IHTMLInput<DateTime?>
    {
        private readonly IJSRuntime _jsRuntime;

        private ElementReference _reference;

        public RenderFragment Render()
        {
            void Fragment(RenderTreeBuilder builder)
            {
                var seq = -1;

                builder.OpenElement(++seq, "input");
                builder.AddAttribute(++seq, "type", "date");
                builder.AddAttribute(++seq, "oninput", EventCallback.Factory.Create(this, Change));

                builder.AddElementReferenceCapture(++seq, r => _reference = r);
                builder.CloseElement();
            }

            return Fragment;
        }

        public HTMLDateInput(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<DateTime?> GetValue()
        {
            string v = await _jsRuntime.InvokeAsync<string>("window.Integrant.Dominant.GetValue", _reference);

            return Parse(v);
        }

        public async Task SetValue(DateTime? value)
        {
            await _jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetValue", _reference,
                value?.ToString("yyyy-MM-dd") ?? "");
        }

        public event Action<DateTime?>? OnChange;

        private void Change(ChangeEventArgs args)
        {
            var v = args.Value?.ToString();

            OnChange?.Invoke(Parse(v));
        }

        private DateTime? Parse(string? v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return null;
            }

            return DateTime.ParseExact(v, "yyyy-MM-dd", new DateTimeFormatInfo());
        }
    }
}