using System.Collections.Generic;

namespace Integrant.Element
{
    public static class BitGetters
    {
        // Structure getters

        public delegate Content BitContent();

        public delegate bool BitIsVisible();

        public delegate string BitURL();

        public delegate bool BitIsChecked();

        public delegate bool BitIsDisabled();

        public delegate IEnumerable<string> BitClasses();

        // Style getters

        public delegate Size BitSize();

        public delegate string BitColor();

        public delegate double BitREM();

        public delegate uint BitPixels();

        public delegate ushort BitWeight();

        public delegate Display BitDisplay();

        // TODO: Implement style
        public delegate bool BitIsHighlighted();
    }
}