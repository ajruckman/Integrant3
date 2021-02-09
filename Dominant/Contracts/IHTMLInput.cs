using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Integrant.Dominant.Contracts
{
    public interface IHTMLInput<T>
    {
        RenderFragment Render();

        Task<T> GetValue();
        Task    SetValue(T value);

        event Action<T>? OnChange;
    }

    public interface IInputRequirable
    {
        Task<bool> IsRequired();
        Task       Require();
        Task       Unrequire();
    }

    public interface IInputDisableable
    {
        Task<bool> IsDisabled();
        Task       Disable();
        Task       Enable();
    }

    public interface IInputWithPlaceholder
    {
        Task<string?> GetPlaceholder();
        Task          SetPlaceholder(string? placeholder);
    }
}