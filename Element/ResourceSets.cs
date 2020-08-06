using Superset.Web.Resources;

namespace Integrant.Element
{
    public static class ResourceSets
    {
        public static readonly ResourceSet Bits = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Bits),
            stylesheets: new[]
            {
                "css/Bits/Bits.css",
                "css/Bits/Bits.css.map",
            }
        );

        public static readonly ResourceSet Constructs = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Constructs),
            stylesheets: new[]
            {
                "css/Constructs/Headers.css",
                "css/Constructs/Headers.css.map",
            }
        );

        public static readonly ResourceSet Layouts = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Layouts),
            stylesheets: new[]
            {
                "css/Layouts/LinearLayout.css",
                "css/Layouts/LinearLayout.css.map",
                "css/Layouts/DropdownLayout.css",
                "css/Layouts/DropdownLayout.css.map",
            }
        );

        public static class Overrides
        {
            public static readonly ResourceSet Buttons = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                nameof(Buttons),
                stylesheets: new[]
                {
                    "css/Overrides/Buttons.css",
                }
            );

            public static readonly ResourceSet Inputs = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                nameof(Inputs),
                stylesheets: new[]
                {
                    "css/Overrides/Inputs.css",
                }
            );

            public static readonly ResourceSet VariantLoader = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                nameof(VariantLoader),
                stylesheets: new[]
                {
                    "css/Overrides/VariantLoader.css",
                }
            );
        }
    }
}