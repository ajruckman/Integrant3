using Superset.Web.Resources;

namespace Integrant.Element
{
    public static class ResourceSets
    {
        public static class Layout
        {
            public static readonly ResourceSet Bits = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                $"{nameof(Layout)}.{nameof(Bits)}",
                stylesheets: new[]
                {
                    "css/Layout/Bits.css",
                    "css/Layout/Bits.css.map",
                }
            );

            public static readonly ResourceSet Layouts = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                $"{nameof(Layout)}.{nameof(Layouts)}",
                stylesheets: new[]
                {
                    "css/Layout/LinearLayout.css",
                    "css/Layout/LinearLayout.css.map",
                    "css/Layout/DropdownLayout.css",
                    "css/Layout/DropdownLayout.css.map",
                }
            );

            public static readonly ResourceSet Headers = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                $"{nameof(Layout)}.{nameof(Headers)}",
                stylesheets: new[]
                {
                    "css/Layout/Headers.css",
                    "css/Layout/Headers.css.map",
                }
            );
        }

        public static class Style
        {
            public static readonly ResourceSet Bits = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                $"{nameof(Style)}.{nameof(Bits)}",
                stylesheets: new[]
                {
                    "css/Style/Bits.css",
                    "css/Style/Bits.css.map",
                }
            );
        }
    }
}