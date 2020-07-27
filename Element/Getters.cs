using System.Collections.Generic;

namespace Integrant.Element
{
    public static class BitGetters
    {
        // Structure getters

        public delegate Content BitContent();

        public delegate bool BitIsVisible();

        public delegate IEnumerable<string> BitClasses();

        public delegate string BitURL();

        // Style getters

        public delegate Size BitSize();

        public delegate string BitColor();

        public delegate double BitREM();

        public delegate uint BitPixels();

        public delegate ushort BitWeight();

        public delegate Display BitDisplay();
    }
}