// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Colorant.Definition
{
    public class ColorRange
    {
        // public int    Steps    { get; set; } = 11;
        public int    HueStart { get; set; }
        public int    HueEnd   { get; set; }
        public string HueCurve { get; set; } = "easeInQuad";
        public int    SatStart { get; set; }
        public int    SatEnd   { get; set; }
        public string SatCurve { get; set; } = "easeOutQuad";
        public int    SatRate  { get; set; } = 100;
        public int    LumStart { get; set; }
        public int    LumEnd   { get; set; }
        public string LumCurve { get; set; } = "easeOutQuad";
        public int    Modifier { get; set; } = 10;
        
        public int HoverStepSize { get; set; }
        public int FocusStepSize { get; set; }
    }
}