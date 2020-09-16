using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Integrant.Element
{
    public static class Interop
    {
        public static async Task CreateBitTooltips(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("Integrant.Element.CreateBitTooltips");
        }
    }
}