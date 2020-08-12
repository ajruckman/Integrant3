using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Components
{
    public class MemberValidations<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.StructureInstance")]
        public StructureInstance<TS> StructureInstance { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        protected override void OnInitialized()
        {
            StructureInstance.ValidationState.OnInvalidation     += () => InvokeAsync(StateHasChanged);
            StructureInstance.ValidationState.OnBeginValidating  += () => InvokeAsync(StateHasChanged);
            StructureInstance.ValidationState.OnFinishValidating += () => InvokeAsync(StateHasChanged);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = StructureInstance.Structure.GetMember<TM>(ID);

            if (member.Validator == null)
                throw new ArgumentNullException(nameof(member.Validator),
                    "Member passed to " + nameof(MemberValidations<TS, TM>) + " component does not have a " +
                    nameof(MemberGetters.MemberValidations<TS, TM>) + ".");

            ClassSet classes = ClassSet.FromMember(StructureInstance.Structure, Value, member,
                "Integrant.Rudiment.Component." + nameof(MemberValidations<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            if (StructureInstance.ValidationState.IsValidating)
            {
                ValidationBuilder.RenderValidatingNotice(builder, ref seq);
            }
            else
            {
                List<Validation>? validations = StructureInstance.ValidationState.GetMemberValidations(ID);

                if (validations != null)
                {
                    foreach (Validation validation in validations)
                    {
                        builder.AddContent(++seq, ValidationBuilder.RenderResult(validation));
                    }
                }
            }

            builder.CloseElement();
        }
    }
}