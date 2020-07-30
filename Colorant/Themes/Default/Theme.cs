using System.Collections.Generic;
using Integrant.Colorant.Schema;
namespace Integrant.Colorant.Themes.Default
{
    public class Theme : ITheme
    {
        public string Assembly { get; } = "Integrant.Colorant";
        public string Name { get; } = "Default";
        public IEnumerable<string> Variants { get; } = new [] { "Dark", "White", "Matrix", "Pink" };
    }
}
