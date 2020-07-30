using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedMember.Global

namespace Integrant.Colorant.Schema
{
    public sealed class Color
    {
        public string         Hex           { get; set; }
        public double         Hue           { get; set; }
        public double         Sat           { get; set; }
        public double         Lum           { get; set; }
        public IList<double?> HSV           { get; set; }
        public IList<double?> HSL           { get; set; }
        public IList<int>     RGB           { get; set; }
        public IList<int>     HueRange      { get; set; }
        public int            Steps         { get; set; }
        public int            Label         { get; set; }
        public string         ContrastBlack { get; set; }
        public string         ContrastWhite { get; set; }
        public string         DisplayColor  { get; set; }
    }
}