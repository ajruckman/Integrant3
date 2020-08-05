using System.Collections.Generic;
using System.Threading.Tasks;
using Integrant.Colorant.Components;
using Integrant.Colorant.Themes.Default;
using Integrant.Element;
using Integrant.Element.Bits;
using Integrant.Element.Constructs;
using Integrant.Fundament;

namespace Integrant.Web.Shared
{
    public partial class MainLayout
    {
        // private ColorSet.Components.ThemeLoader _themeLoader;
        private Layer  _rootLayer;
        private Header _header1 = null!;

        private ThemeLoader _defaultThemeLoader;
        private ThemeLoader _solidsThemeLoader;

        protected override void OnInitialized()
        {
            _rootLayer = new Layer();

            // _themeLoader            =  new ColorSet.Components.ThemeLoader(StorageService, Configuration.ResourceSet, "Dark");
            // _themeLoader.OnComplete += StateHasChanged;

            _defaultThemeLoader = new ThemeLoader(StorageService, new Theme(),
                Variants.Dark.ToString());
            _defaultThemeLoader.OnComplete += StateHasChanged;

            _solidsThemeLoader = new ThemeLoader(StorageService, new Colorant.Themes.Solids.Theme(),
                Colorant.Themes.Solids.Variants.Normal.ToString());

            //
            
            _header1 = new Header
            (
                new List<IBit>
                {
                    new Title(() => "Header #1!"),
                    new Filler(),
                    new Link(() => "Link root", () => "/"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url2"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url3"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url4"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url5"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url6"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url7"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url8"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url9"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url10"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url11"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url12"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url13"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url14"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url15"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url16"),
                    new Space(),
                    new Separator(),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url17"),
                },
                doHighlight: true
            );
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