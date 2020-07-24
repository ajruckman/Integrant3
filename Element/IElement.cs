using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public interface IElement
    {
        public RenderFragment Render();
    }
}