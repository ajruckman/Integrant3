using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Components
{
    public class Button : ComponentBase
    {
        private Bits.Button? _button;
        
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            // _button = new Bits.Button(() => ChildContent);
        }
    }
}