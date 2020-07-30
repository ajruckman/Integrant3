using System.IO;
using Integrant.Colorant.Schema;
using Newtonsoft.Json;

namespace Integrant.Web.Pages
{
    public partial class Colorant
    {
        private ThemeDefinition _theme = null!;

        protected override void OnInitialized()
        {
            _theme = JsonConvert.DeserializeObject<ThemeDefinition>
            (
                File.ReadAllText("../Colorant/Themes/Default/Compiled.json")
            );
        }
    }
}