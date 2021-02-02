using Superset.Web.Resources;

namespace Integrant.Dominant
{
    public static class ResourceSets
    {
        public static readonly ResourceSet Interop = new ResourceSet
        (
            $"{nameof(Integrant)}.{nameof(Dominant)}",
            nameof(Interop),
            scripts: new[] {"Dominant.js"}
        );
    }
}