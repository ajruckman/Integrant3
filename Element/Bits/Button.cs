using System;
using System.Linq;
using System.Threading.Tasks;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Button : BitBase
    {
        public delegate Color ColorGetter();

        public enum Color
        {
            Default,
            Blue,
            Green,
            Orange,
            Purple,
            Red,
            Yellow,
        }

        private readonly Func<ClickArgs, Task> _onClick;
        private readonly ColorGetter           _color;

        public Button
        (
            BitGetters.BitContent        content,
            Func<ClickArgs, Task>        onClick,
            ColorGetter?                 color           = null,
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
            BitGetters.BitIsHighlighted? isHighlighted   = null,
            BitGetters.BitData?          data            = null
        )
        {
            Spec = new BitSpec
            {
                Content         = content,
                IsStatic        = isStatic,
                IsVisible       = isVisible,
                IsDisabled      = isDisabled,
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
                IsHighlighted   = isHighlighted,
                Data            = data,
            };

            _onClick = onClick;
            _color   = color ?? DefaultColorGetter;

            ConstantClasses = new ClassSet(
                "Integrant.Element.Override.Button",
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Button)
            );

            // ConstantClasses.Add("Integrant.Element.Override.Button:" + color.inv);

            Cache(additionalClasses: LocalClasses());
        }

        public Button
        (
            BitGetters.BitContent        content,
            Action<ClickArgs>            onClick,
            ColorGetter?                 color           = null,
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
            BitGetters.BitIsHighlighted? isHighlighted   = null,
            BitGetters.BitData?          data            = null
        ) : this(
            content,
            async v =>
            {
                onClick.Invoke(v);
                await Task.CompletedTask;
            },
            color,
            isStatic,
            isVisible,
            isDisabled,
            classes,
            margin,
            padding,
            foregroundColor,
            backgroundColor,
            pixelsHeight,
            pixelsWidth,
            fontSize,
            fontWeight,
            display,
            isHighlighted,
            data
        ) { }

        private static Color DefaultColorGetter() => Color.Default;

        private string[] LocalClasses()
        {
            string[] result = {"Integrant.Element.Override." + nameof(Button) + ":" + _color.Invoke()};

            if (Spec.IsHighlighted?.Invoke() == true)
                result = result.Append("Integrant.Element.Override." + nameof(Button) + ":Highlighted").ToArray();

            return result;
        }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            BitBuilder.OpenElement(builder, ref seq, "button", this, null, LocalClasses());

            builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, Click));

            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);

            BitBuilder.CloseElement(builder);
        };

        private async Task Click(MouseEventArgs args)
        {
            await _onClick.Invoke(new ClickArgs
            (
                (ushort) args.Button,
                (ushort) args.ClientX,
                (ushort) args.ClientY,
                args.ShiftKey,
                args.CtrlKey
            ));
        }
    }
}