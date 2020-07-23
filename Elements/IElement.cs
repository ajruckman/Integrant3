using Microsoft.AspNetCore.Components;

namespace Integrant.Elements
{
    public interface IElement
    {
        public RenderFragment Render();
    }
}