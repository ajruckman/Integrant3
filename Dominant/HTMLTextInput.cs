using System;
using System.Threading.Tasks;
using Integrant.Dominant.Contracts;
using Integrant.Dominant.Managers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Integrant.Dominant
{
    public partial class HTMLTextInput : IHTMLInput<string?>, IInputRequirable, IInputDisableable, IInputWithPlaceholder
    {
        private readonly IJSRuntime       _jsRuntime;
        private          ElementReference _reference;
        private          string?          _value;

        public HTMLTextInput(IJSRuntime jsRuntime, string? value, bool disabled, bool required, string? placeholder)
        {
            _jsRuntime = jsRuntime;

            _value = value;

            _inputDisabledManager    = new InputDisabledManager(jsRuntime, disabled);
            _inputRequiredManager    = new InputRequiredManager(jsRuntime, required);
            _inputPlaceholderManager = new InputPlaceholderManager(jsRuntime, placeholder);
        }

        public RenderFragment Render()
        {
            void Fragment(RenderTreeBuilder builder)
            {
                Console.WriteLine("RENDER");
                var seq = -1;

                builder.OpenElement(++seq, "input");
                builder.AddAttribute(++seq, "type", "text");
                builder.AddAttribute(++seq, "value", _value);
                builder.AddAttribute(++seq, "oninput", EventCallback.Factory.Create(this, Change));
                builder.AddAttribute(++seq, "placeholder", _inputPlaceholderManager.GetPlaceholder());
                builder.AddAttribute(++seq, "disabled", _inputDisabledManager.IsDisabled());
                builder.AddAttribute(++seq, "required", _inputRequiredManager.IsRequired());

                builder.AddElementReferenceCapture(++seq, r => _reference = r);
                builder.CloseElement();
            }

            return Fragment;
        }

        public async Task<string?> GetValue()
        {
            var value = await _jsRuntime.InvokeAsync<string>("window.Integrant.Dominant.GetValue", _reference);
            _value = string.IsNullOrEmpty(value) ? null : value;
            return _value;
        }

        public async Task SetValue(string? value)
        {
            _value = value == "" ? null : value;
            await _jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetValue", _reference, value ?? "");
            OnChange?.Invoke(_value);
        }

        public event Action<string?>? OnChange;

        private void Change(ChangeEventArgs args)
        {
            OnChange?.Invoke(args.Value?.ToString());
        }
    }

    public partial class HTMLTextInput
    {
        private readonly InputDisabledManager _inputDisabledManager;

        public       Task<bool> IsDisabled() => Task.FromResult(_inputDisabledManager.IsDisabled());
        public async Task       Disable()    => await _inputDisabledManager.Disable(_reference);
        public async Task       Enable()     => await _inputDisabledManager.Enable(_reference);
    }

    public partial class HTMLTextInput
    {
        private readonly InputRequiredManager _inputRequiredManager;

        public       Task<bool> IsRequired() => Task.FromResult(_inputRequiredManager.IsRequired());
        public async Task       Require()    => await _inputRequiredManager.Require(_reference);
        public async Task       Unrequire()  => await _inputRequiredManager.Unrequire(_reference);
    }

    public partial class HTMLTextInput
    {
        private readonly InputPlaceholderManager _inputPlaceholderManager;

        public Task<string?> GetPlaceholder() => Task.FromResult(_inputPlaceholderManager.GetPlaceholder());

        public async Task SetPlaceholder(string? placeholder) =>
            await _inputPlaceholderManager.SetPlaceholder(_reference, placeholder);
    }
}