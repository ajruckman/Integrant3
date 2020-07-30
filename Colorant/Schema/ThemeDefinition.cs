using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Integrant.Colorant.Schema
{
    public class ThemeDefinition
    {
        [JsonConstructor]
        public ThemeDefinition(string name, List<Block> blocks, List<Variant> variants)
        {
            Name     = name;
            Blocks   = blocks;
            Variants = variants;
        }

        public string        Name     { get; }
        public List<Block>   Blocks   { get; }
        public List<Variant> Variants { get; }
    }

    public class Block
    {
        [JsonConstructor]
        public Block(string name, List<string> ids, bool createDisplayTextVariables)
        {
            Name                       = name;
            IDs                        = ids;
            CreateDisplayTextVariables = createDisplayTextVariables;
        }

        public string       Name                       { get; }
        public List<string> IDs                        { get; }
        public bool         CreateDisplayTextVariables { get; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum VariantBlockColorSource
    {
        Undefined,
        Given,
        Range,
    }

    public class Variant
    {
        [JsonConstructor]
        public Variant
        (
            string                                          name,
            Dictionary<string, VariantBlockColorSource>     blockSources,
            Dictionary<string, Dictionary<string, string>>? blockColorsGiven,
            Dictionary<string, ColorRange>?                 blockColorsRange,
            string?                                         defaultDarkTextColor,
            string?                                         defaultLightTextColor
        )
        {
            Name                  = name;
            BlockSources          = blockSources;
            BlockColorsGiven      = blockColorsGiven;
            BlockColorsRange      = blockColorsRange;
            DefaultDarkTextColor  = defaultDarkTextColor;
            DefaultLightTextColor = defaultLightTextColor;
        }

        public string Name { get; }

        public string? DefaultDarkTextColor  { get; }
        public string? DefaultLightTextColor { get; }

        public Dictionary<string, VariantBlockColorSource>     BlockSources     { get; }
        public Dictionary<string, Dictionary<string, string>>? BlockColorsGiven { get; }
        public Dictionary<string, ColorRange>?                 BlockColorsRange { get; }

        public Dictionary<string, Dictionary<string, string>>? Colors { get; set; }
    }
}