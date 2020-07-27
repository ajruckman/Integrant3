using Integrant.Fundament;
using Integrant.Rudiment.Inputs;
using Microsoft.AspNetCore.Components;

namespace Integrant.Web.Components
{
    public partial class ElementTests
    {
        [CascadingParameter]
        public Layer Layer { get; set; }

        private Layer _childLayer;

        public double Number { get; set; }
        
        protected override void OnInitialized()
        {
            _childLayer = new Layer(Layer);
        }
    }
}