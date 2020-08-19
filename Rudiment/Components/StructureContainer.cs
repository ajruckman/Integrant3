using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Components
{
    public class StructureContainer<TS> : ComponentBase
    {
        [Parameter]
        public StructureInstance<TS> StructureInstance { get; set; } = null!;

        [Parameter]
        public TS Value { get; set; } = default!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ClassSet classes = ClassSet.FromStructure(StructureInstance.Structure, Value,
                "Integrant.Rudiment.Component." + nameof(StructureContainer<TS>));

            bool shown = StructureInstance.Structure.IsVisible?.Invoke(StructureInstance.Structure, Value) ?? true;

            //

            int seq = -1;

            builder.OpenComponent<CascadingValue<StructureInstance<TS>>>(++seq);
            builder.AddAttribute(++seq, "Name",    "Integrant.Rudiment.StructureInstance");
            builder.AddAttribute(++seq, "Value",   StructureInstance);
            builder.AddAttribute(++seq, "IsFixed", true);
            builder.AddAttribute(++seq, "ChildContent", new RenderFragment(builder2 =>
            {
                builder2.OpenComponent<CascadingValue<TS>>(++seq);
                builder2.AddAttribute(++seq, "Name",    "Integrant.Rudiment.Value");
                builder2.AddAttribute(++seq, "Value",   Value);
                builder2.AddAttribute(++seq, "IsFixed", false);
                builder2.AddAttribute(++seq, "ChildContent", new RenderFragment(builder3 =>
                {
                    builder3.OpenElement(++seq, "div");
                    builder3.AddAttribute(++seq, "class", classes.ToString());

                    if (!shown)
                        builder3.AddAttribute(++seq, "hidden", "hidden");

                    builder3.AddContent(++seq, ChildContent);

                    builder3.CloseElement();
                }));

                builder2.CloseComponent();
            }));
            builder.CloseComponent();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                StructureInstance.ValidateInitial(Value);
        }
    }
}