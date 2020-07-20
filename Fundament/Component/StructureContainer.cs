using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fundament.Component
{
    public class StructureContainer<T> : ComponentBase
    {
        [Parameter]
        public Structure<T> Structure { get; set; } = null!;

        [Parameter]
        public T Value { get; set; } = default!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var classes = new List<string> {"Fundament.Component." + nameof(StructureContainer<T>)};

            if (Structure.StructureClasses != null)
                classes.AddRange(Structure.StructureClasses.Invoke(Structure, Value));

            bool shown = Structure.StructureIsVisible?.Invoke(Structure, Value) ?? true;

            //

            int seq = -1;

            builder.OpenComponent<CascadingValue<Structure<T>>>(++seq);
            builder.AddAttribute(++seq, "Name",    "Fundament.Structure");
            builder.AddAttribute(++seq, "Value",   Structure);
            builder.AddAttribute(++seq, "IsFixed", true);
            builder.AddAttribute(++seq, "ChildContent", new RenderFragment(builder2 =>
            {
                builder2.OpenComponent<CascadingValue<T>>(++seq);
                builder2.AddAttribute(++seq, "Name",    "Fundament.Value");
                builder2.AddAttribute(++seq, "Value",   Value);
                builder2.AddAttribute(++seq, "IsFixed", false);
                builder2.AddAttribute(++seq, "ChildContent", new RenderFragment(builder3 =>
                {
                    builder3.OpenElement(++seq, "div");
                    builder3.AddAttribute(++seq, "class", string.Join(' ', classes));

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
                Structure.ValidateInitial(Value);
        }
    }
}