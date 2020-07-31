using System.Collections.Generic;
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
                    // ColorSet.ResourceSets.Globals,
                    FontSet.ResourceSets.Inter,
                    FontSet.ResourceSets.JetBrainsMono,
                    Rudiment.ResourceSets.Layout.Validations,
                    Rudiment.ResourceSets.Layout.Inputs,
                    Rudiment.ResourceSets.Layout.Components,
                    Rudiment.ResourceSets.Style.Inputs,
                    Rudiment.ResourceSets.Style.Colorant,
                    Element.ResourceSets.Layout.Bits,
                    Element.ResourceSets.Layout.Layouts,
                    Element.ResourceSets.Style.Bits,
                    
                    Resources.ResourceSets.MaterialIcons,
                });
        }
    }
}