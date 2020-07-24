using System.Threading.Tasks;
using ColorSet.Components;

namespace Integrant.Web.Shared
{
    public partial class MainLayout
    {
        private ThemeLoader _themeLoader;

        protected override void OnInitialized()
        {
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