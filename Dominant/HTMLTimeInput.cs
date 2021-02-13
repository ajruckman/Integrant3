using System;
using System.Globalization;
using System.Threading.Tasks;
using Integrant.Dominant.Contracts;
using Integrant.Dominant.Managers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Integrant.Dominant
{
    public partial class HTMLTimeInput : IHTMLInput<DateTime?>, IInputRequirable, IInputDisableable
    {
        private readonly IJSRuntime       _jsRuntime;
        private          ElementReference _reference;
        private          DateTime?        _value;

        public HTMLTimeInput(IJSRuntime jsRuntime, DateTime? value, bool disabled, bool required)
        {
            _jsRuntime = jsRuntime;

            _value = value;

            _inputDisabledManager = new InputDisabledManager(jsRuntime, disabled);
            _inputRequiredManager = new InputRequiredManager(jsRuntime, required);
        }

        public RenderFragment Render()
        {
            void Fragment(RenderTreeBuilder builder)
            {
                var seq = -1;

                builder.OpenElement(++seq, "input");
                builder.AddAttribute(++seq, "type", "time");
                builder.AddAttribute(++seq, "value", _value?.ToString("HH:mm"));
                builder.AddAttribute(++seq, "oninput", EventCallback.Factory.Create(this, Change));

                builder.AddElementReferenceCapture(++seq, r => _reference = r);
                builder.CloseElement();
            }

            return Fragment;
        }

        public async Task<DateTime?> GetValue()
        {
            var v = await _jsRuntime.InvokeAsync<string>("window.Integrant.Dominant.GetValue", _reference);
            _value = Parse(v);
            return _value;
        }

        public async Task SetValue(DateTime? value)
        {
            _value = value == null || value == DateTime.MinValue ? null : value;
            await _jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetValue", _reference,
                value?.ToString("HH:mm") ?? "");
            OnChange?.Invoke(_value);
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

            return DateTime.ParseExact(v, "HH:mm", new DateTimeFormatInfo());
        }
    }

    public partial class HTMLTimeInput
    {
        private readonly InputDisabledManager _inputDisabledManager;

        public       Task<bool> IsDisabled() => Task.FromResult(_inputDisabledManager.IsDisabled());
        public async Task       Disable()    => await _inputDisabledManager.Disable(_reference);
        public async Task       Enable()     => await _inputDisabledManager.Enable(_reference);
    }

    public partial class HTMLTimeInput
    {
        private readonly InputRequiredManager _inputRequiredManager;

        public       Task<bool> IsRequired() => Task.FromResult(_inputRequiredManager.IsRequired());
        public async Task       Require()    => await _inputRequiredManager.Require(_reference);
        public async Task       Unrequire()  => await _inputRequiredManager.Unrequire(_reference);
    }
}