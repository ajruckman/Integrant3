using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public interface IConstruct
    {
        public RenderFragment Render();
    }
}