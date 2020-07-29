using System;
using System.Collections.Generic;
using Colorant.ColorGeneratorInterop;
using Colorant.Definition;

namespace Colorant
{
    public class Generator
    {
        public void Generate(ThemeDefinition themeDefinition)
        {
            var caller = new ColorGeneratorCaller();

            foreach (Variant variant in themeDefinition.Variants)
            {
                foreach (Block block in themeDefinition.Blocks)
                {
                    if (!variant.BlockSources.ContainsKey(block.Name)) continue;

                    VariantBlockColorSource source = variant.BlockSources[block.Name];
                    if (source == VariantBlockColorSource.Undefined) continue;

                    Dictionary<string, string> blockColors = new Dictionary<string, string>();

                    variant.Colors ??= new Dictionary<string, Dictionary<string, string>>();

                    if (source == VariantBlockColorSource.Range)
                    {
                        if (variant.BlockColorsRange?.ContainsKey(block.Name) != true) continue;

                        ColorRange  range = variant.BlockColorsRange![block.Name];
                        List<Color> r     = caller.Call(block, range);

                        for (var i = 0; i < r.Count; i++)
                        {
                            blockColors[i.ToString()] = r[i].Hex;
                            Console.WriteLine($"{variant.Name} -> {block.Name} -> {i} = {r[i].Hex}");
                        }
                    }
                    else if (source == VariantBlockColorSource.Given)
                    {
                        if (variant.BlockColorsGiven?.ContainsKey(block.Name) != true) continue;

                        foreach (string blockID in block.IDs)
                        {
                            if (!variant.BlockColorsGiven![block.Name].ContainsKey(blockID))
                                throw new Exception($"Given color missing for block '{block.Name}' ID {blockID}.");

                            string hex = variant.BlockColorsGiven![block.Name][blockID];
                            blockColors[blockID] = hex;
                            Console.WriteLine($"{variant.Name} -> {block.Name} -> {blockID} = {hex}");
                        }
                    }

                    variant.Colors[block.Name] = blockColors;
                }
            }
        }
    }
}