using System;
using System.Collections.Generic;
using System.IO;
using Colorant.Definition;

namespace Colorant
{
    public class Writer
    {
        public void Write(ThemeDefinition themeDefinition, string constantNamespace)
        {
            var hardcodedConstantBlockNameLines = new List<string>();
            hardcodedConstantBlockNameLines.Add($"namespace {constantNamespace}");
            hardcodedConstantBlockNameLines.Add("{");
            hardcodedConstantBlockNameLines.Add("\tpublic static class ");
            
            foreach (Block block in themeDefinition.Blocks)
            {
                
            }

            foreach (Variant variant in themeDefinition.Variants)
            {
                if (variant.Colors == null) continue;

                var cssGlobalVarLines = new List<string>();
                cssGlobalVarLines.Add(":root {");

                foreach (Block block in themeDefinition.Blocks)
                {
                    if (!variant.Colors.ContainsKey(block.Name)) continue;

                    foreach (var (id, color) in variant.Colors[block.Name])
                    {
                        cssGlobalVarLines.Add(
                            $"\t--Colorant_{themeDefinition.Name}_{variant.Name}_{block.Name}_{id}: {color};");
                    }
                }

                cssGlobalVarLines.Add("}");

                cssGlobalVarLines.ForEach(Console.WriteLine);

                File.WriteAllLines($"Themes/{themeDefinition.Name}.{variant.Name}.css", cssGlobalVarLines);
            }
        }
    }
}