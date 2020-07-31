using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Resources.Icons.MaterialIcons
{
    public sealed class Icon : ComponentBase
    {
        [Parameter]
        public string ID { get; set; } = null!;

        [Parameter]
        public ushort Size { get; set; } = 24;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;

            builder.OpenElement(++seq, "i");
            builder.AddAttribute(++seq, "style", $"font-size: {Size}px;");
            builder.AddAttribute(++seq, "class",
                "Integrant.Resources.Icon Integrant.Resources.Icon:MaterialIcons material-icons");
            builder.AddContent(++seq, ID);
            builder.CloseElement();

            // builder.OpenElement(++seq, "svg");
            // builder.AddAttribute(++seq, "style", $"height: {Size}px; width: {Size}px;");
            // builder.OpenElement(++seq, "use");
            // builder.AddAttribute(++seq, "href", $"_content/Integrant.Resources/css.gg/all.svg#gg-{ID}");
            // builder.CloseElement();
            // builder.CloseElement();
        }
    }
}