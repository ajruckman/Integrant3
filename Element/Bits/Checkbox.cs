using System;
using Integrant.Fundament;
using Integrant.Resources.Icons.MaterialIcons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Superset.Web.State;

namespace Integrant.Element.Bits
{
    public class Checkbox : Bit
    {
        private readonly Action<bool>  _onToggle;
        private          bool          _checked;
        private readonly UpdateTrigger _trigger = new UpdateTrigger();

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
            BitGetters.BitColor?      foregroundColor = null,
            BitGetters.BitColor?      backgroundColor = null,
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
                ForegroundColor = foregroundColor,
                BackgroundColor = backgroundColor,
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

            Cache(additionalClasses: LocalClasses());

            _onToggle = onToggle;
            _checked  = isChecked?.Invoke() ?? false;
        }

        private string[]? LocalClasses()
        {
            return Spec.IsDisabled?.Invoke() == true
                ? new[] {"Integrant.Element.Bit." + nameof(Checkbox) + ":Disabled"}
                : null;
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "style",   Style(false));
            builder.AddAttribute(++seq, "class",   Class(false, LocalClasses()));
            builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClick));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.OpenComponent<TriggerWrapper>(++seq);
            builder.AddAttribute(++seq, "Trigger", _trigger);
            builder.AddAttribute(++seq, "ChildContent", new RenderFragment(builder2 =>
            {
                builder2.OpenComponent<Icon>(++seq);
                builder2.AddAttribute(++seq, "ID", !_checked ? "check_box_outline_blank" : "check_box");
                builder2.CloseComponent();
            }));
            builder.CloseComponent();

            builder.CloseElement();
        };

        private void OnClick(MouseEventArgs obj)
        {
            if (Spec.IsDisabled?.Invoke() == true)
                return;

            _checked = !_checked;
            _trigger.Trigger();
            
            _onToggle.Invoke(_checked);
        }
    }
}