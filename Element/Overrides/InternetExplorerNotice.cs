using Microsoft.AspNetCore.Http;

namespace Integrant.Element.Overrides
{
    public static class InternetExplorerNotice
    {
        public static bool IsIE(HttpRequest request)
        {
            string userAgent = request.Headers["User-Agent"];

            return userAgent.Contains("MSIE") || userAgent.Contains("Trident");
        }

        public const string StylesheetURL =
            "/_content/"      + nameof(Integrant)              +
            "."               + nameof(Element)                +
            "/css/Overrides/" + nameof(InternetExplorerNotice) + ".css";
    }
}