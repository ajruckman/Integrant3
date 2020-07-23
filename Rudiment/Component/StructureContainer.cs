using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Component
{
    public class StructureContainer<TS> : ComponentBase
    {
        [Parameter]
        public StructureState<TS> StructureState { get; set; } = null!;

        [Parameter]
        public TS Value { get; set; } = default!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ClassSet classes = ClassSet.FromStructure(StructureState.Structure, Value,
                "Integrant.Rudiment.Component." + nameof(StructureContainer<TS>));

            bool shown = StructureState.Structure.IsVisible?.Invoke(StructureState.Structure, Value) ?? true;

            //

            int seq = -1;

            builder.OpenComponent<CascadingValue<StructureState<TS>>>(++seq);
            builder.AddAttribute(++seq, "Name",    "Integrant.Rudiment.StructureState");
            builder.AddAttribute(++seq, "Value",   StructureState);
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
                    builder3.AddAttribute(++seq, "class", classes.Format());

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
                StructureState.ValidateInitial(Value);
        }
    }
}