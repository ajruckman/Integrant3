using System;
using System.Threading.Tasks;
using Integrant.Fundament;
using Integrant.Resources.Icons.MaterialIcons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Superset.Web.State;

namespace Integrant.Element.Bits
{
    public class Checkbox : BitBase
    {
        private readonly Func<bool, Task> _onToggle;
        private readonly UpdateTrigger    _trigger = new UpdateTrigger();

        public Checkbox
        (
            Func<bool, Task>          onToggle,
            BitGetters.BitIsChecked?  isChecked       = null,
            BitGetters.BitIsDisabled? isDisabled      = null,
            bool                      isStatic        = true,
            BitGetters.BitIsVisible?  isVisible       = null,
            BitGetters.BitClasses?    classes         = null,
            BitGetters.BitSize?       margin          = null,
            BitGetters.BitSize?       padding         = null,
            BitGetters.BitColor?      backgroundColor = null,
            BitGetters.BitColor?      foregroundColor = null,
            BitGetters.BitPixels?     pixelsHeight    = null,
            BitGetters.BitPixels?     pixelsWidth     = null,
            BitGetters.BitREM?        fontSize        = null,
            BitGetters.BitWeight?     fontWeight      = null,
            BitGetters.BitDisplay?    display         = null
        )
        {
            Spec = new BitSpec
            {
                IsChecked       = isChecked,
                IsDisabled      = isDisabled,
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                Classes         = classes,
                Margin          = margin,
                Padding         = padding,
                BackgroundColor = backgroundColor,
                ForegroundColor = foregroundColor,
                PixelsHeight    = pixelsHeight,
                PixelsWidth     = pixelsWidth,
                FontSize        = fontSize,
                FontWeight      = fontWeight,
                Display         = display,
            };

            ConstantClasses = new ClassSet(
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Checkbox)
            );

            Cache();

            _onToggle = onToggle;
            Checked   = isChecked?.Invoke() ?? false;
        }

        public Checkbox
        (
            Action<bool>              onToggle,
            BitGetters.BitIsChecked?  isChecked       = null,
            BitGetters.BitIsDisabled? isDisabled      = null,
            bool                      isStatic        = true,
            BitGetters.BitIsVisible?  isVisible       = null,
            BitGetters.BitClasses?    classes         = null,
            BitGetters.BitSize?       margin          = null,
            BitGetters.BitSize?       padding         = null,
            BitGetters.BitColor?      backgroundColor = null,
            BitGetters.BitColor?      foregroundColor = null,
            BitGetters.BitPixels?     pixelsHeight    = null,
            BitGetters.BitPixels?     pixelsWidth     = null,
            BitGetters.BitREM?        fontSize        = null,
            BitGetters.BitWeight?     fontWeight      = null,
            BitGetters.BitDisplay?    display         = null
        ) : this(
            async v =>
            {
                onToggle.Invoke(v);
                await Task.CompletedTask;
            },
            isChecked,
            isDisabled,
            isStatic,
            isVisible,
            classes,
            margin,
            padding,
            backgroundColor,
            foregroundColor,
            pixelsHeight,
            pixelsWidth,
            fontSize,
            fontWeight,
            display
        ) { }

        public bool Checked { get; private set; }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "style",   Style(false));
            builder.AddAttribute(++seq, "class",   Class(false));
            builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClick));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.OpenComponent<TriggerWrapper>(++seq);
            builder.AddAttribute(++seq, "Trigger", _trigger);
            builder.AddAttribute(++seq, "ChildContent", new RenderFragment(builder2 =>
            {
                builder2.OpenComponent<Icon>(++seq);
                builder2.AddAttribute(++seq, "ID", !Checked ? "check_box_outline_blank" : "check_box");
                builder2.CloseComponent();
            }));
            builder.CloseComponent();

            builder.CloseElement();
        };

        private async Task OnClick(MouseEventArgs obj)
        {
            if (Spec.IsDisabled?.Invoke() == true)
                return;

            Checked = !Checked;
            _trigger.Trigger();

            await _onToggle.Invoke(Checked);

            // bool? now = Spec.IsChecked?.Invoke();
            // if (now == null) return;
            //
            // if (_checked != now.Value)
            // {
            //     _checked = now.Value;
            //     _onToggle.Invoke(_checked);
            // } 
        }

        public void Reset()
        {
            Checked = Spec.IsChecked?.Invoke() ?? false;
            _trigger.Trigger();
        }
    }
}