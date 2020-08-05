namespace Integrant.Fundament
{
    public readonly struct ClickArgs
    {
        public ClickArgs(ushort button, ushort x, ushort y, bool shift, bool control)
        {
            Button  = button;
            X       = x;
            Y       = y;
            Shift   = shift;
            Control = control;
        }

        public readonly ushort Button;
        public readonly ushort X;
        public readonly ushort Y;
        public readonly bool   Shift;
        public readonly bool   Control;
    }
}