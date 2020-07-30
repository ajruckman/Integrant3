using System;
using System.Collections.Generic;
using Integrant.Colorant.ColorGeneratorInterop;
using Integrant.Colorant.Schema;

namespace Integrant.Colorant
{
    public class Generator
    {
        public void Generate(ThemeDefinition themeDefinition)
        {
            var caller = new ColorGeneratorCaller();

            foreach (Variant variant in themeDefinition.Variants)
            {
                Console.WriteLine($"{variant.Name} =>");
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

                        string colorBoxURL =
                            $"https://www.colorbox.io/#steps={block.IDs.Count}" +
                            $"#hue_start={range.HueStart}#hue_end={range.HueEnd}#hue_curve={range.HueCurve}" +
                            $"#sat_start={range.SatStart}#sat_end={range.SatEnd}#sat_curve={range.SatCurve}#sat_rate={range.SatRate}" +
                            $"#lum_start={range.LumStart}#lum_end={range.LumEnd}#lum_curve={range.LumCurve}";
                        Console.WriteLine($"{block.Name} = {colorBoxURL}");

                        for (var i = 0; i < r.Count; i++)
                        {
                            Color c = r[i];
                            blockColors[i.ToString()] = c.Hex;

                            if (!block.CreateDisplayTextVariables) continue;
                            
                            switch (c.DisplayColor)
                            {
                                case "black":
                                    if (variant.DefaultDarkTextColor != null)
                                        blockColors[$"{i}_Text"] = variant.DefaultDarkTextColor;
                                    break;
                                case "white":
                                    if (variant.DefaultLightTextColor != null)
                                        blockColors[$"{i}_Text"] = variant.DefaultLightTextColor;
                                    break;
                            }

                            // Console.WriteLine($"{variant.Name} -> {block.Name} -> {i} = {r[i].Hex}");
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
                            // Console.WriteLine($"{variant.Name} -> {block.Name} -> {blockID} = {hex}");
                        }
                    }

                    variant.Colors[block.Name] = blockColors;
                }

                Console.WriteLine();
            }
        }
    }
}