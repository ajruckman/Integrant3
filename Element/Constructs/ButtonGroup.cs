using System.Collections.Generic;
using Integrant.Element.Bits;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Constructs
{
    public class ButtonGroup : IConstruct
    {
        private readonly List<Button> _buttons;
        private readonly string       _classes;

        public ButtonGroup
        (
            List<Button>? buttons = null
        )
        {
            _buttons = buttons ?? new List<Button>();
            _classes = "Integrant.Element.Construct Integrant.Element.Construct.ButtonGroup";
        }

        public void Add(Button button) => _buttons.Add(button);

        public RenderFragment Render() => builder =>
        {
            int seq = -1;
            
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classes);

            foreach (Button button in _buttons)
            {
                builder.AddContent(++seq, button.Render());
            }
            
            builder.CloseElement();
        };
    }
}