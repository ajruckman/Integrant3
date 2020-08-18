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
                    Rudiment.ResourceSets.Layout.Components,

                    Element.ResourceSets.Overrides.BlazorErrorUI,
                    Element.ResourceSets.Overrides.BlazorReconnectModal,
                    Element.ResourceSets.Overrides.Buttons,
                    Element.ResourceSets.Overrides.Inputs,
                    Element.ResourceSets.Overrides.VariantLoader,
                    Element.ResourceSets.Arrangements,
                    Element.ResourceSets.Bits,
                    Element.ResourceSets.Components,
                    Element.ResourceSets.Layouts,
                    Element.ResourceSets.Constructs,

                    Resources.ResourceSets.MaterialIcons,
                    Superset.Web.ResourceSets.LocalCSS,
                });
        }
    }
}