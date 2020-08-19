// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Integrant.Element
{
    internal class BitSpec
    {
        internal bool                         IsStatic;
        internal BitGetters.BitContent?       Content;
        internal BitGetters.BitIsVisible?     IsVisible;
        internal BitGetters.BitIsChecked?     IsChecked;
        internal BitGetters.BitIsDisabled?    IsDisabled;
        internal BitGetters.BitClasses?       Classes;
        internal BitGetters.BitURL?           URL;
        internal BitGetters.BitSize?          Margin;
        internal BitGetters.BitSize?          Padding;
        internal BitGetters.BitColor?         BackgroundColor;
        internal BitGetters.BitColor?         ForegroundColor;
        internal BitGetters.BitPixels?        PixelsHeight;
        internal BitGetters.BitPixels?        PixelsWidth;
        internal BitGetters.BitREM?           FontSize;
        internal BitGetters.BitWeight?        FontWeight;
        internal BitGetters.BitDisplay?       Display;
        internal BitGetters.BitIsHighlighted? IsHighlighted;
    }
}