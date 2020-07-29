using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Colorant.Definition
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

        public string        Name     { get; set; }
        public List<Block>   Blocks   { get; set; }
        public List<Variant> Variants { get; set; }
    }

    public class Block
    {
        [JsonConstructor]
        public Block(string name, List<string> ds)
        {
            Name = name;
            IDs  = ds;
        }

        public string       Name { get; set; }
        public List<string> IDs  { get; set; }
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
            Dictionary<string, ColorRange>?                 blockColorsRange
        )
        {
            Name             = name;
            BlockSources     = blockSources;
            BlockColorsGiven = blockColorsGiven;
            BlockColorsRange = blockColorsRange;
        }

        // public Theme  Theme { get; set; }
        public string Name { get; set; }

        public Dictionary<string, VariantBlockColorSource>     BlockSources     { get; set; }
        public Dictionary<string, Dictionary<string, string>>? BlockColorsGiven { get; set; }
        public Dictionary<string, ColorRange>?                 BlockColorsRange { get; set; }

        public Dictionary<string, Dictionary<string, string>>? Colors { get; set; }
    }
}