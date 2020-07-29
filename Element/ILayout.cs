using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public interface ILayout
    {
        public List<IBit>     Contents { get; }
        public void           Add(IBit bit);
        public RenderFragment Render();
    }
}