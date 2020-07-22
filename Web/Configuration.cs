using System.Collections.Generic;
using Superset.Web.Resources;

namespace Integrant.Web
{
    internal sealed class Configuration
    {
        internal readonly ResourceSet ResourceSet;

        internal Configuration()
        {
            ResourceSet = new ResourceSet(nameof(Integrant) + "." + nameof(Web), nameof(Configuration),
                dependencies: new List<ResourceSet>
                {
                    Superset.Web.ResourceSets.Listeners,
                    Superset.Web.ResourceSets.LocalCSS,
                    FlareSelect.ResourceSets.FlareSelect,
                    FlareTables.ResourceSets.FlareTables,
                });
        }
    }
}