using System.Collections.Generic;
using System.Diagnostics;
using Colorant.Definition;
using Newtonsoft.Json;

namespace Colorant.ColorGeneratorInterop
{
    internal sealed class ColorGeneratorCaller
    {
        private readonly Process _p;

        public ColorGeneratorCaller()
        {
            _p = new Process
            {
                StartInfo =
                {
                    UseShellExecute        = false,
                    RedirectStandardInput  = true,
                    RedirectStandardOutput = true,
                    FileName               = "node.exe",
                    Arguments              = "./ColorGenerator/index.js",
                }
            };
            _p.Start();
        }

        public List<Color> Call(Block block, ColorRange s)
        {
            _p.StandardInput.WriteLine(
                $"{block.IDs.Count} "                                +
                $"{s.HueStart} {s.HueEnd} {s.HueCurve} "             +
                $"{s.SatStart} {s.SatEnd} {s.SatCurve} {s.SatRate} " +
                $"{s.LumStart} {s.LumEnd} {s.LumCurve} "             +
                $"{s.Modifier}");

            string json = _p.StandardOutput.ReadLine();

            var range = JsonConvert.DeserializeObject<List<Color>>
            (
                json,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            );

            return range;
        }
    }
}