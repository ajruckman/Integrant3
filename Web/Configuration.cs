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
                    Resources.ResourceSets.Fonts.SansSerif.Inter,
                    Resources.ResourceSets.Fonts.Monospaced.JetBrainsMono,

                    Superset.Web.ResourceSets.Listeners,
                    FlareSelect.ResourceSets.FlareSelect,
                    FlareTables.ResourceSets.FlareTables,
                    // ColorSet.ResourceSets.Globals,
                    Rudiment.ResourceSets.Layout.Validations,
                    Rudiment.ResourceSets.Layout.Inputs,
                    Rudiment.ResourceSets.Layout.Components,
                    Rudiment.ResourceSets.Style.Inputs,
                    Rudiment.ResourceSets.Style.Colorant,
                    Element.ResourceSets.Layout.Bits,
                    Element.ResourceSets.Layout.Layouts,
                    Element.ResourceSets.Layout.Headers,
                    Element.ResourceSets.Style.Bits,

                    Resources.ResourceSets.MaterialIcons,
                    Superset.Web.ResourceSets.LocalCSS,
                });
        }
    }
}