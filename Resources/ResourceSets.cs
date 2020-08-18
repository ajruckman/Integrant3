using Superset.Web.Resources;

namespace Integrant.Resources
{
    public static class ResourceSets
    {
        public static readonly ResourceSet MaterialIcons = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Resources)}",
            nameof(MaterialIcons),
            stylesheets: new[] {"MaterialIcons/Icons.css"}
        );

        public static class Fonts
        {
            public static class SansSerif
            {
                public static readonly ResourceSet Inter = new ResourceSet
                (
                    $"{nameof(Integrant)}.{nameof(Resources)}",
                    $"{nameof(Fonts)}.{nameof(SansSerif)}.{nameof(Inter)}",
                    stylesheets: new[] {"Font/Inter/Inter.css"}
                );

                public static readonly ResourceSet Roboto = new ResourceSet
                (
                    $"{nameof(Integrant)}.{nameof(Resources)}",
                    $"{nameof(Fonts)}.{nameof(SansSerif)}.{nameof(Roboto)}",
                    stylesheets: new[] {"Font/Roboto/Roboto.css"}
                );
            }

            public static class Monospaced
            {
                public static readonly ResourceSet JetBrainsMono = new ResourceSet
                (
                    $"{nameof(Integrant)}.{nameof(Resources)}",
                    $"{nameof(Fonts)}.{nameof(Monospaced)}.{nameof(JetBrainsMono)}",
                    stylesheets: new[] {"Font/JetBrainsMono/JetBrainsMono.css"}
                );

                public static readonly ResourceSet RobotoMono = new ResourceSet
                (
                    $"{nameof(Integrant)}.{nameof(Resources)}",
                    $"{nameof(Fonts)}.{nameof(Monospaced)}.{nameof(RobotoMono)}",
                    stylesheets: new[] {"Font/RobotoMono/RobotoMono.css"}
                );
            }
        }

        public static class Libraries
        {
            public static readonly ResourceSet Popper = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Resources)}",
                $"{nameof(Libraries)}.{nameof(Popper)}",
                scripts: new[]
                {
                    "Libraries/Popper/popper.min.js",
                    // "Libraries/Popper/popper.interop.js",
                }
            );
        }
    }
}