using System.Collections.Generic;
using Integrant.Colorant.Schema;
namespace Integrant.Colorant.Themes.Solids
{
    public class Theme : ITheme
    {
        public string Assembly { get; } = "Integrant.Colorant";
        public string Name { get; } = "Solids";
        public IEnumerable<string> Variants { get; } = new [] { "Normal" };
    }
}
