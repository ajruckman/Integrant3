using System.Collections.Generic;
using Integrant.Rudiment;
using Superset.Web.Resources;

namespace Integrant.Web
{
    internal sealed class Configuration
    {
        internal static readonly ResourceSet ResourceSet;

        static Configuration()
        {
            ResourceSet = new ResourceSet(nameof(Integrant) + "." + nameof(Web), nameof(Configuration),
                dependencies: new List<ResourceSet>
                {
                    Superset.Web.ResourceSets.Listeners,
                    Superset.Web.ResourceSets.LocalCSS,
                    FlareSelect.ResourceSets.FlareSelect,
                    FlareTables.ResourceSets.FlareTables,
                    ColorSet.ResourceSets.Globals,
                    FontSet.ResourceSets.Inter,
                    FontSet.ResourceSets.JetBrainsMono,
                    ResourceSets.Layout.Validations,
                    ResourceSets.Layout.Inputs,
                    ResourceSets.Layout.Components,
                    ResourceSets.Style.Inputs,
                });
        }
    }
}