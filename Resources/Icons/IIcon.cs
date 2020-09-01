using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Resources.Icons
{
    public interface IIcon : IComponent, IHandleEvent, IHandleAfterRender
    {
        public string ID   { get; }
        public ushort Size { get; }
    }
}