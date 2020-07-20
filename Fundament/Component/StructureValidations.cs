using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fundament.Component
{
    public class StructureValidations<TS> : ComponentBase
    {
        [CascadingParameter(Name = "Fundament.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Fundament.Value")]
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
            List<string> classes = new List<string> {"Fundament.Component." + nameof(StructureValidations<TS>)};

            if (Structure.StructureValidator == null)
                throw new ArgumentNullException(nameof(Structure.StructureValidator),
                    "Structure passed to " + nameof(StructureValidations<TS>) + " component does not have a " +
                    nameof(Getters.StructureValidations<TS>) + ".");

            if (Structure.StructureClasses != null)
                classes.AddRange(Structure.StructureClasses.Invoke(Structure, Value));

            bool shown = Structure.StructureIsVisible?.Invoke(Structure, Value) ?? true;

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            if (Structure.ValidationState.IsValidating)
            {
                builder.OpenElement(++seq, "div");
                builder.AddAttribute(++seq, "class", "Fundament.ValidationNotice.Validating");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "Fundament.ValidationNotice.Validating.Background");
                builder.AddContent(++seq, "Validating...");
                builder.CloseElement();
                builder.CloseElement();
            }
            else
            {
                List<Validation>? validations = Structure.ValidationState.GetStructureValidations();

                if (validations != null)
                {
                    Console.WriteLine("Exists");
                    foreach (Validation validation in validations)
                    {
                        builder.AddContent(++seq, validation.Render());
                    }
                }
            }

            builder.CloseElement();
        }
    }
}