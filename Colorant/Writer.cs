using System.Collections.Generic;
using System.IO;
using System.Linq;
using Integrant.Colorant.Schema;

namespace Integrant.Colorant
{
    public static class Writer
    {
        private const string I = "    ";
        private const string C = I + I + "public const string ";

        public static void Write(ThemeDefinition theme, string assembly)
        {
            var constLines = new List<string>();

            foreach (Block block in theme.Blocks)
            {
                foreach (string id in block.IDs)
                {
                    constLines.Add(
                        C + $"{block.Name}_{id} = \"var(--Colorant_{theme.Name}_{block.Name}_{id})\";");

                    if (!block.CreateDisplayTextVariables) continue;

                    constLines.Add(
                        C + $"{block.Name}_{id}_Text = \"var(--Colorant_{theme.Name}_{block.Name}_{id}_Text)\";");
                }
            }

            File.WriteAllLines($"Themes/{theme.Name}/Constants.cs", new List<string>
            {
                $"namespace {assembly}.Themes.{theme.Name}",
                "{",
                I + "public static class Constants",
                I + "{",
                string.Join('\n', constLines),
                I + "}",
                "}",
            });

            //

            var variantLines = new List<string>();

            foreach (Variant variant in theme.Variants)
            {
                variantLines.Add(I + I + $"{variant.Name},");
            }

            File.WriteAllLines($"Themes/{theme.Name}/Variants.cs", new List<string>
            {
                $"namespace {assembly}.Themes.{theme.Name}",
                "{",
                I + "public enum Variants",
                I + "{",
                I + I + "Undefined,",
                string.Join('\n', variantLines),
                I + "}",
                "}",
            });

            //

            string variants = "\"" + string.Join("\", \"", theme.Variants.Select(v => v.Name)) + "\"";

            File.WriteAllLines($"Themes/{theme.Name}/Theme.cs", new List<string>
            {
                "using System.Collections.Generic;",
                "using Integrant.Colorant.Schema;",
                $"namespace {assembly}.Themes.{theme.Name}",
                "{",
                I + "public class Theme : ITheme",
                I + "{",
                I + I + $"public string Assembly {{ get; }} = \"{assembly}\";",
                I + I + $"public string Name {{ get; }} = \"{theme.Name}\";",
                I + I + $"public IEnumerable<string> Variants {{ get; }} = new [] {{ {variants} }};",
                I + "}",
                "}",
            });

            //

            foreach (Variant variant in theme.Variants)
            {
                if (variant.Colors == null) continue;

                var cssLines = new List<string>();
                cssLines.Add(":root {");

                foreach (Block block in theme.Blocks)
                {
                    if (!variant.Colors.ContainsKey(block.Name)) continue;

                    foreach (var (id, color) in variant.Colors[block.Name])
                    {
                        cssLines.Add(
                            $"\t--Colorant_{theme.Name}_{block.Name}_{id}: {color};");
                    }
                }

                cssLines.Add("}");

                Directory.CreateDirectory($"wwwroot/css/{theme.Name}");

                File.WriteAllLines($"wwwroot/css/{theme.Name}/{variant.Name}.css", cssLines);
            }
        }
    }
}