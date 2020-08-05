using System.Collections.Generic;
using System.IO;
using Integrant.Colorant.Schema;
using Newtonsoft.Json;

namespace Integrant.Web.Pages
{
    public partial class Colorant
    {
        private readonly List<ThemeDefinition> _themes = new List<ThemeDefinition>();

        protected override void OnInitialized()
        {
            _themes.Add(JsonConvert.DeserializeObject<ThemeDefinition>
            (
                File.ReadAllText("../Colorant/Themes/Default/Compiled.json")
            ));
            _themes.Add(JsonConvert.DeserializeObject<ThemeDefinition>
            (
                File.ReadAllText("../Colorant/Themes/Solids/Compiled.json")
            ));
        }
    }
}