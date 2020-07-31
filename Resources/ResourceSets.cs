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
    }
}