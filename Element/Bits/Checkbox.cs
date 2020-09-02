using System;
using System.Threading.Tasks;
using Integrant.Fundament;
using Integrant.Resources.Icons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Integrant.Element.Bits
{
    public class Checkbox : BitBase
    {
        private readonly Func<bool, Task> _onToggle;

        public Checkbox
        (
            Func<bool, Task>          onToggle,
            BitGetters.BitIsChecked?  isChecked       = null,
            BitGetters.BitIsDisabled? isDisabled      = null,
            BitGetters.BitIsRequired? isRequired      = null,
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
                IsRequired      = isRequired,
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

            Cache(null, AdditionalClasses());

            _onToggle = onToggle;
            Checked   = isChecked?.Invoke() ?? false;
        }

        public Checkbox
        (
            Action<bool>              onToggle,
            BitGetters.BitIsChecked?  isChecked       = null,
            BitGetters.BitIsDisabled? isDisabled      = null,
            BitGetters.BitIsRequired? isRequired      = null,
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
            isRequired,
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

        private string[]? AdditionalClasses()
        {
            return Spec.IsRequired?.Invoke() == true && !Checked
                ? new[] {"Integrant.Element.Bit:FailsRequirement"}
                : null;
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenComponent<Component>(++seq);
            builder.AddAttribute(++seq, "Checkbox", this);
            builder.CloseComponent();
        };

        private Action? StateHasChanged;

        private class Component : ComponentBase
        {
            [Parameter] public Checkbox Checkbox { get; set; } = null!;

            protected override void OnInitialized()
            {
                Checkbox.StateHasChanged = () => InvokeAsync(StateHasChanged);
            }

            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                int seq = -1;

                BitBuilder.OpenElement(builder, ref seq, "div", Checkbox, null, Checkbox.AdditionalClasses());

                builder.AddAttribute(++seq, "onclick",
                    EventCallback.Factory.Create<MouseEventArgs>(this, Checkbox.OnClick));
                builder.OpenComponent<MaterialIcon>(++seq);
                builder.AddAttribute(++seq, "ID", !Checkbox.Checked ? "check_box_outline_blank" : "check_box");
                builder.CloseComponent();
                
                BitBuilder.CloseElement(builder);
            }
        }

        private async Task OnClick(MouseEventArgs obj)
        {
            if (Spec.IsDisabled?.Invoke() == true)
                return;

            Checked = !Checked;
            (StateHasChanged ?? throw ReconstructedException).Invoke();

            await _onToggle.Invoke(Checked);
        }

        public void Reset()
        {
            Checked = Spec.IsChecked?.Invoke() ?? false;
            (StateHasChanged ?? throw ReconstructedException).Invoke();
        }
    }
}