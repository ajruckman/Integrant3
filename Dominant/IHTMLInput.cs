using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Integrant.Dominant
{
    public interface IHTMLInput<T>
    {
        RenderFragment Render();
        
        Task<T> GetValue();
        Task SetValue(T value);

        event Action<T>? OnChange;
    }
}