// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Integrant.Element
{
    internal class BitSpec
    {
        internal bool                         IsStatic        { get; set; }
        internal BitGetters.BitContent?       Content         { get; set; }
        internal BitGetters.BitIsVisible?     IsVisible       { get; set; }
        internal BitGetters.BitIsChecked?     IsChecked       { get; set; }
        internal BitGetters.BitIsDisabled?    IsDisabled      { get; set; }
        internal BitGetters.BitClasses?       Classes         { get; set; }
        internal BitGetters.BitURL?           URL             { get; set; }
        internal BitGetters.BitSize?          Margin          { get; set; }
        internal BitGetters.BitSize?          Padding         { get; set; }
        internal BitGetters.BitColor?         ForegroundColor { get; set; }
        internal BitGetters.BitColor?         BackgroundColor { get; set; }
        internal BitGetters.BitPixels?        PixelsHeight    { get; set; }
        internal BitGetters.BitPixels?        PixelsWidth     { get; set; }
        internal BitGetters.BitREM?           FontSize        { get; set; }
        internal BitGetters.BitWeight?        FontWeight      { get; set; }
        internal BitGetters.BitDisplay?       Display         { get; set; }
        internal BitGetters.BitIsHighlighted? IsHighlighted   { get; set; }
    }
}