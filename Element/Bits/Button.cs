using System;
using System.Threading.Tasks;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

// ReSharper disable ArgumentsStyleNamedExpression

namespace Integrant.Element.Bits
{
    public class Button : BitBase
    {
        public enum Color
        {
            Blue,
            Green,
            Orange,
            Purple,
            Red,
            Yellow,
        }

        private readonly Func<ClickArgs, Task> _onClick;

        public Button
        (
            BitGetters.BitContent     content,
            Func<ClickArgs, Task>     onClick,
            Color?                    color           = null,
            bool?                     isStatic        = null,
            BitGetters.BitIsVisible?  isVisible       = null,
            BitGetters.BitIsDisabled? isDisabled      = null,
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
                Content         = content,
                IsStatic        = isStatic == true || isDisabled == null,
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
            };

            _onClick = onClick;

            ConstantClasses = new ClassSet(
                "Integrant.Element.Button",
                "Integrant.Element.Bit",
                "Integrant.Element.Bit." + nameof(Button)
            );

            if (color == null)
                ConstantClasses.Add("Integrant.Element.Button:Default");
            else
                ConstantClasses.Add("Integrant.Element.Button:" + color);

            Cache();
        }

        public Button
        (
            BitGetters.BitContent     content,
            Action<ClickArgs>         onClick,
            Color?                    color           = null,
            bool?                     isStatic        = null,
            BitGetters.BitIsVisible?  isVisible       = null,
            BitGetters.BitIsDisabled? isDisabled      = null,
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
            display
        ) { }

        public override RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "button");
            builder.AddAttribute(++seq, "style", Style(false));
            builder.AddAttribute(++seq, "class", Class(false));

            ++seq;
            if (Spec.IsVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.AddAttribute(++seq, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, Click));

            builder.AddContent(++seq, Spec.Content!.Invoke().Fragment);
            builder.CloseElement();
        };

        private void Click(MouseEventArgs args)
        {
            _onClick.Invoke(new ClickArgs
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