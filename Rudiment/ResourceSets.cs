using Superset.Web.Resources;

namespace Integrant.Rudiment
{
    public static class ResourceSets
    {
        public static class Layout
        {
            public static readonly ResourceSet Validations = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Validations)}",
                stylesheets: new[]
                {
                    "css/Layout/Validation.css",
                    // "css/Layout/Validation.css.map",
                },
                dependencies: new[] {Resources.ResourceSets.MaterialIcons}
            );

            public static readonly ResourceSet Components = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(Layout)}.{nameof(Components)}",
                stylesheets: new[]
                {
                    "css/Layout/Component.MemberValue.css",
                    // "css/Layout/Component.MemberValue.css.map",
                }
            );
        }
    }
}