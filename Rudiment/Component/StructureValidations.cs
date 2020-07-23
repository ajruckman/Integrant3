using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Component
{
    public class StructureValidations<TS> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        protected override void OnInitialized()
        {
            Structure.ValidationState.OnInvalidation += () =>
            {
                Console.WriteLine("! StructureValidations -> OnInvalidation");
                InvokeAsync(StateHasChanged);
            };
            Structure.ValidationState.OnBeginValidating += () =>
            {
                Console.WriteLine("! StructureValidations -> OnBeginValidating");
                InvokeAsync(StateHasChanged);
            };
            Structure.ValidationState.OnFinishValidatingStructure += () =>
            {
                Console.WriteLine("! StructureValidations -> OnFinishValidatingStructure");
                InvokeAsync(StateHasChanged);
            };
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Structure.Validator == null)
                throw new ArgumentNullException(nameof(Structure.Validator),
                    "Structure passed to " + nameof(StructureValidations<TS>) + " component does not have a " +
                    nameof(StructureGetters.StructureValidations<TS>) + ".");

            ClassSet classes = ClassSet.FromStructure(Structure, Value,
                "Integrant.Rudiment.Component." + nameof(StructureContainer<TS>));

            bool shown = Structure.IsVisible?.Invoke(Structure, Value) ?? true;

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            if (Structure.ValidationState.IsValidating)
            {
                ValidationBuilder.RenderValidatingNotice(builder, ref seq);
            }
            else
            {
                List<Validation>? validations = Structure.ValidationState.GetStructureValidations();

                if (validations != null)
                {
                    Console.WriteLine("Exists");
                    foreach (Validation validation in validations)
                    {
                        ValidationBuilder.RenderResult(builder, ref seq, validation);
                    }
                }
            }

            builder.CloseElement();
        }
    }
}