using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Components
{
    public class StructureValidations<TS> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.StructureInstance")]
        public StructureInstance<TS> StructureInstance { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        protected override void OnInitialized()
        {
            StructureInstance.ValidationState.OnInvalidation += () => InvokeAsync(StateHasChanged);
            StructureInstance.ValidationState.OnBeginValidating += () => InvokeAsync(StateHasChanged);
            StructureInstance.ValidationState.OnFinishValidating += () => InvokeAsync(StateHasChanged);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (StructureInstance.Structure.Validator == null)
                throw new ArgumentNullException(nameof(StructureInstance.Structure.Validator),
                    "Structure passed to " + nameof(StructureValidations<TS>) + " component does not have a " +
                    nameof(StructureGetters.StructureValidations<TS>) + ".");

            ClassSet classes = ClassSet.FromStructure(StructureInstance.Structure, Value,
                "Integrant.Rudiment.Component." + nameof(StructureValidations<TS>));

            bool shown = StructureInstance.Structure.IsVisible?.Invoke(StructureInstance.Structure, Value) ?? true;

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            if (StructureInstance.ValidationState.IsValidating)
            {
                ValidationBuilder.RenderValidatingNotice(builder, ref seq);
            }
            else
            {
                List<Validation>? validations = StructureInstance.ValidationState.GetStructureValidations();

                if (validations != null)
                {
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