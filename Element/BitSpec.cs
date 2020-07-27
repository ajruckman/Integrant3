using System;
using System.Linq;
using System.Reflection;
using Integrant.Element.Bits;

namespace Integrant.Element
{
    public class BitSpec
    {
        // TODO: Use init-only auto properties in C# 9

        public bool IsStatic;

        [BitUsage(nameof(Chip), nameof(Heading))]
        public BitGetters.BitContent? Content;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitIsVisible? IsVisible;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitClasses? Classes;

        [BitUsage()]
        public BitGetters.BitURL? URL;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitSize? Margin;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitSize? Padding;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitColor? ForegroundColor;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitColor? BackgroundColor;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitPixels? PixelsHeight;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitPixels? PixelsWidth;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitREM? FontSize;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitWeight? FontWeight;

        [BitUsage(nameof(Arrow), nameof(Chip), nameof(Heading))]
        public BitGetters.BitDisplay? Display;

        internal void ValidateFor(string bit)
        {
            foreach (FieldInfo field in GetType().GetFields())
            {
                var usage = field.GetCustomAttribute<BitUsage>();

                if (usage == null)
                    continue;

                bool hasValue = field.GetValue(this) != null;

                if (hasValue && !usage.Permitted.Contains(bit))
                {
                    throw new Exception($"Getter '{field.Name}' cannot be used in Bit '{bit}'.");
                }
            }
        }
    }
}