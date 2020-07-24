using System.Threading.Tasks;
using ColorSet.Components;
using Integrant.Fundament;

namespace Integrant.Web.Shared
{
    public partial class MainLayout
    {
        private ThemeLoader _themeLoader;
        private Layer       _rootLayer;

        protected override void OnInitialized()
        {
            _rootLayer = new Layer();
            
            _themeLoader            =  new ThemeLoader(StorageService, Configuration.ResourceSet, "Dark");
            _themeLoader.OnComplete += StateHasChanged;
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _themeLoader.Load();
            }
        }
    }
}