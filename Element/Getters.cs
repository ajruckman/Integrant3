namespace Integrant.Element
{
    public static class BitGetters
    {
        public delegate Content BitContent();
        
        public delegate bool BitIsVisible();

        public delegate string BitBackgroundColor();

        public delegate string BitTextColor();

        public delegate Size BitMargin();

        public delegate Size BitPadding();

        public delegate double BitREM();

        public delegate ushort BitWeight();

        public delegate byte BitPxHeight();

        public delegate byte BitPxWidth();

        public delegate byte BitPxThickness();

        public delegate string BitURL();
    }
}