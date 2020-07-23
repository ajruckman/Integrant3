using Superset.Web.Resources;

namespace Integrant.Rudiment
{
    public static class ResourceSets
    {
        public static class CSS
        {
            public static readonly ResourceSet Icons = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(CSS)}.{nameof(Icons)}",
                stylesheets: new[] {"vendor/material-design-icons/iconfont/material-icons.css", "css/Icons.css"}
            );

            public static readonly ResourceSet Inputs = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(CSS)}.{nameof(Inputs)}",
                stylesheets: new[] {"css/Inputs.css"}
            );

            public static readonly ResourceSet Validations = new ResourceSet
            (
                $"{nameof(Integrant)}.{nameof(Rudiment)}",
                $"{nameof(CSS)}.{nameof(Validations)}",
                stylesheets: new[] {"css/Validation.css"},
                dependencies: new[] {Icons}
            );
        }
    }
}