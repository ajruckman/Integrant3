using Superset.Web.Resources;

namespace Integrant.Rudiment
{
    public static class ResourceSets
    {
        public static class Layout
        {
            public static readonly ResourceSet Inputs = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Inputs)}",
                stylesheets: new[] {"css/Layout/Inputs.css"}
            );

            public static readonly ResourceSet Validations = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Validations)}",
                stylesheets: new[] {"css/Layout/Validation.css"},
                dependencies: new[] {Resources.ResourceSets.MaterialIcons}
            );

            public static readonly ResourceSet Components = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Components)}",
                stylesheets: new[]
                {
                    "css/Layout/Component.MemberValue.css",
                }
            );
        }

        public static class Style
        {
            public static readonly ResourceSet Inputs = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Inputs)}",
                stylesheets: new[] {"css/Style/Inputs.css"}
            );

            public static readonly ResourceSet Colorant = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Colorant)}",
                stylesheets: new[] {"css/Style/Colorant.css"}
            );
        }
    }
}