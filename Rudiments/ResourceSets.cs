using Superset.Web.Resources;

namespace Integrant.Rudiments
{
    public static class ResourceSets
    {
        public static class Layout
        {
            public static readonly ResourceSet Icons = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiments)}",
                $"{nameof(Layout)}.{nameof(Icons)}",
                stylesheets: new[] {"vendor/material-design-icons/iconfont/material-icons.css", "css/Layout/Icons.css"}
            );

            public static readonly ResourceSet Inputs = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiments)}",
                $"{nameof(Layout)}.{nameof(Inputs)}",
                stylesheets: new[] {"css/Layout/Inputs.css"}
            );

            public static readonly ResourceSet Validations = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiments)}",
                $"{nameof(Layout)}.{nameof(Validations)}",
                stylesheets: new[] {"css/Layout/Validation.css"},
                dependencies: new[] {Icons}
            );

            public static readonly ResourceSet Components = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiments)}",
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
                $"{nameof(Integrant)}.{nameof(Rudiments)}",
                $"{nameof(Layout)}.{nameof(Inputs)}",
                stylesheets: new[] {"css/Style/Inputs.css"}
            );

        }
    }
}