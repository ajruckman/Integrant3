using System.Collections.Generic;
using Integrant.Element.Bits;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Constructs
{
    public class ButtonGroup : IConstruct
    {
        private const string Classes = "Integrant.Element.Construct Integrant.Element.Construct.ButtonGroup";

        private readonly List<Button> _buttons;

        public ButtonGroup
        (
            List<Button>? buttons = null
        )
        {
            _buttons = buttons ?? new List<Button>();
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", Classes);

            foreach (Button button in _buttons)
            {
                builder.AddContent(++seq, button.Render());
            }

            builder.CloseElement();
        };

        // public ButtonGroup
        // (
        //     List<ToggleButton>? buttons = null
        // )
        // {
        //     _buttons = buttons.Cast<>() ?? new List<IBit>();
        //     _classes = "Integrant.Element.Construct Integrant.Element.Construct.ButtonGroup";
        // }

        public void Add(Button button) => _buttons.Add(button);
    }
}