using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Integrant.Dominant
{
    public abstract class InputBase<T>
    {
        protected readonly IJSRuntime       JSRuntime;
        protected          ElementReference Reference;
        protected          T                Value;

        protected InputBase(IJSRuntime jsRuntime, T value)
        {
            JSRuntime = jsRuntime;
            Value     = value;
        }

        public async Task<T> GetValue()
        {
            var v = await JSRuntime.InvokeAsync<string>("window.Integrant.Dominant.GetValue", Reference);
            Value = Parse(v);
            return Value;
        }

        public async Task SetValue(T? value)
        {
            Value = NullCheck(value);
            await JSRuntime.InvokeVoidAsync("window.Integrant.Dominant.SetValue", Reference, Encode(value));
            OnChange?.Invoke(Value);
        }

        protected void InvokeOnChange(T value) => OnChange?.Invoke(value);

        public event Action<T>? OnChange;

        protected abstract T      Parse(string? v);
        protected abstract string Encode(T?     v);
        protected abstract T      NullCheck(T?  v);
    }
}