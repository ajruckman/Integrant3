using System;
using System.Threading.Tasks;
using Integrant.Element.Bits;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;
using Superset.Web.State;

namespace Integrant.Element.Constructs
{
    public class ToggleButton : IConstruct
    {
        private readonly Button           _button;
        private readonly Func<bool, Task> _onToggle;
        private readonly UpdateTrigger    _trigger = new UpdateTrigger();
        private readonly string           _classes;

        public ToggleButton
        (
            BitGetters.BitContent        content,
            Func<bool, Task>             onToggle,
            BitGetters.BitIsChecked?     isToggled       = null,
            bool                         isStatic        = false,
            BitGetters.BitIsVisible?     isVisible       = null,
            BitGetters.BitIsDisabled?    isDisabled      = null,
            BitGetters.BitClasses?       classes         = null,
            BitGetters.BitSize?          margin          = null,
            BitGetters.BitSize?          padding         = null,
            BitGetters.BitColor?         foregroundColor = null,
            BitGetters.BitColor?         backgroundColor = null,
            BitGetters.BitPixels?        pixelsHeight    = null,
            BitGetters.BitPixels?        pixelsWidth     = null,
            BitGetters.BitREM?           fontSize        = null,
            BitGetters.BitWeight?        fontWeight      = null,
            BitGetters.BitDisplay?       display         = null,
            BitGetters.BitIsHighlighted? isHighlighted   = null
        )
        {
            _button = new Button
            (
                content,
                _ => Toggle(),
                isStatic: isStatic,
                isVisible: isVisible,
                isDisabled: isDisabled,
                classes: classes,
                margin: margin,
                padding: padding,
                foregroundColor: foregroundColor,
                backgroundColor: backgroundColor,
                pixelsHeight: pixelsHeight,
                pixelsWidth: pixelsWidth,
                fontSize: fontSize,
                fontWeight: fontWeight,
                display: display,
                isHighlighted: () => Toggled
            );

            _classes  = "Integrant.Element.Bit Integrant.Element.Bit." + nameof(Toggle);
            _onToggle = onToggle;
            Toggled   = isToggled?.Invoke() ?? false;
        }

        public bool Toggled { get; private set; }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classes);

            builder.OpenComponent<TriggerWrapper>(++seq);
            builder.AddAttribute(++seq, "Trigger",      _trigger);
            builder.AddAttribute(++seq, "ChildContent", _button.Render());
            builder.CloseComponent();

            builder.CloseElement();
        };

        private void Toggle()
        {
            Toggled = !Toggled;
            _onToggle.Invoke(Toggled);
            _trigger.Trigger();
        }
    }
}