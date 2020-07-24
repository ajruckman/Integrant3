using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Web.Pages
{
    public partial class ElementTests
    {
        [CascadingParameter]
        public Layer Layer { get; set; }

        private Layer _childLayer;
        
        protected override void OnInitialized()
        {
            _childLayer = new Layer(Layer);
        }
    }
}