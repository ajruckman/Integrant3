using System.Threading.Tasks;
using Integrant.Colorant.Components;
using Integrant.Fundament;

namespace Integrant.Web.Shared
{
    public partial class MainLayout
    {
        // private ColorSet.Components.ThemeLoader _themeLoader;
        private Layer _rootLayer;

        private ThemeLoader _defaultThemeLoader;
        private ThemeLoader _solidsThemeLoader;

        protected override void OnInitialized()
        {
            _rootLayer = new Layer();

            // _themeLoader            =  new ColorSet.Components.ThemeLoader(StorageService, Configuration.ResourceSet, "Dark");
            // _themeLoader.OnComplete += StateHasChanged;

            _defaultThemeLoader = new ThemeLoader(StorageService, new Colorant.Themes.Default.Theme(),
                Colorant.Themes.Default.Variants.Dark.ToString());
            _defaultThemeLoader.OnComplete += StateHasChanged;

            _solidsThemeLoader = new ThemeLoader(StorageService, new Colorant.Themes.Solids.Theme(),
                Colorant.Themes.Solids.Variants.Normal.ToString());
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // await _themeLoader.Load();
                await _defaultThemeLoader.Load();
            }
        }
    }
}