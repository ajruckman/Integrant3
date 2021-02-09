using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Integrant.Dominant.Managers
{
    public class InputDisabledManager
    {
        private readonly IJSRuntime _jsRuntime;
        private          bool       _disabled;

        public InputDisabledManager(IJSRuntime jsRuntime, bool disabled)
        {
            _jsRuntime = jsRuntime;
            _disabled  = disabled;
        }

        public bool IsDisabled() => _disabled;

        public async Task Disable(ElementReference reference) => await SetDisabled(reference, true);
        public async Task Enable(ElementReference  reference) => await SetDisabled(reference, false);

        private async Task SetDisabled(ElementReference reference, bool disabled)
        {
            _disabled = disabled;
            await _jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetDisabled", reference, disabled);
        }
    }

    public class InputRequiredManager
    {
        private readonly IJSRuntime _jsRuntime;
        private          bool       _required;

        public InputRequiredManager(IJSRuntime jsRuntime, bool required)
        {
            _jsRuntime = jsRuntime;
            _required  = required;
        }

        public bool IsRequired() => _required;

        public async Task Require(ElementReference   reference) => await SetRequired(_jsRuntime, reference, true);
        public async Task Unrequire(ElementReference reference) => await SetRequired(_jsRuntime, reference, false);

        private async Task SetRequired(IJSRuntime jsRuntime, ElementReference reference, bool required)
        {
            _required = required;
            await jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetRequired", reference, required);
        }
    }

    public class InputPlaceholderManager
    {
        private readonly IJSRuntime _jsRuntime;
        private          string?    _placeholder;

        public InputPlaceholderManager(IJSRuntime jsRuntime, string? placeholder)
        {
            _jsRuntime   = jsRuntime;
            _placeholder = placeholder;
        }

        public string? GetPlaceholder() => _placeholder;

        public async Task SetPlaceholder(ElementReference reference, string? placeholder)
        {
            _placeholder = placeholder == "" ? null : placeholder;
            await _jsRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetPlaceholder", reference, placeholder ?? "");
        }
    }
}