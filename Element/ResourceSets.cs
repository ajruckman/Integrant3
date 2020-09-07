using Superset.Web.Resources;

namespace Integrant.Element
{
    public static class ResourceSets
    {
        public static readonly ResourceSet Arrangements = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Arrangements),
            stylesheets: new[]
            {
                "css/Arrangements/Arrangements.css",
            }
        );

        public static readonly ResourceSet Bits = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Bits),
            stylesheets: new[]
            {
                "css/Bits/Bits.css",
            }
        );

        public static readonly ResourceSet Components = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Components),
            stylesheets: new[]
            {
                "css/Components/Components.css",
                "css/Components/TabbedPanel.css",
                "css/Components/Combobox.css",
                "css/Components/Modal.css",
                "css/Components/Multibox.css",
            },
            scripts: new[]
            {
                "js/Components/Combobox.js",
            },
            dependencies: new[]
            {
                Resources.ResourceSets.Libraries.Popper,
            }
        );

        public static readonly ResourceSet Constructs = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Constructs),
            stylesheets: new[]
            {
                "css/Constructs/ButtonGroup.css",
                "css/Constructs/Dropdown.css",
                "css/Constructs/HeaderGroup.css",
                "css/Constructs/Header.css",
            }
        );

        public static readonly ResourceSet Layouts = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Element)}",
            nameof(Layouts),
            stylesheets: new[]
            {
                "css/Layouts/LinearLayout.css",
                "css/Layouts/VerticalLayout.css",
            }
        );

        public static class Overrides
        {
            public static readonly ResourceSet BlazorErrorUI = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                nameof(BlazorErrorUI),
                stylesheets: new[]
                {
                    "css/Overrides/BlazorErrorUI.css",
                }
            );

            public static readonly ResourceSet BlazorReconnectModal = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Element)}",
                nameof(BlazorReconnectModal),
                stylesheets: new[]
                {
                    "css/Overrides/BlazorReconnectModal.css",
                }
            );

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