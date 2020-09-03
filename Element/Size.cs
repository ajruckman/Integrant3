namespace Integrant.Element
{
    public readonly struct Size
    {
        public readonly byte Top;
        public readonly byte Right;
        public readonly byte Bottom;
        public readonly byte Left;

        public Size(byte all)
        {
            Top = Right = Bottom = Left = all;
        }

        public Size(byte vertical, byte horizontal)
        {
            Top   = Bottom = vertical;
            Right = Left   = horizontal;
        }

        public Size(byte top, byte horizontal, byte bottom)
        {
            Top    = top;
            Right  = Left = horizontal;
            Bottom = bottom;
        }

        public Size(byte top, byte horizontal, byte bottom, byte left)
        {
            Top    = top;
            Right  = Left = horizontal;
            Bottom = bottom;
            Left   = left;
        }
    }
}