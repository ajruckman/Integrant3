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
                stylesheets: new[] {"css/Layout/Bits.css"}
            );

            public static readonly ResourceSet Layouts = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                $"{nameof(Layout)}.{nameof(Layouts)}",
                stylesheets: new[] {"css/Layout/LinearLayout.css"}
            );
        }

        public static class Style
        {
            public static readonly ResourceSet Bits = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                $"{nameof(Style)}.{nameof(Bits)}",
                stylesheets: new[] {"css/Style/Bits.css"}
            );
        }
    }
}