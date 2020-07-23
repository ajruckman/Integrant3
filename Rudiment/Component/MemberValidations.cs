using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Component
{
    public class MemberValidations<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        protected override void OnInitialized()
        {
            Structure.ValidationState.OnInvalidation += () =>
            {
                Console.WriteLine($"! MemberValidations @ {ID} -> OnInvalidation");
                InvokeAsync(StateHasChanged);
            };
            Structure.ValidationState.OnBeginValidating += () =>
            {
                Console.WriteLine($"! MemberValidations @ {ID} -> OnBeginValidating");
                InvokeAsync(StateHasChanged);
            };
            Structure.ValidationState.OnFinishValidatingStructure += () =>
            {
                Console.WriteLine($"! MemberValidations @ {ID} -> OnFinishValidatingStructure");
                InvokeAsync(StateHasChanged);
            };
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = Structure.GetMember<TM>(ID);

            if (member.Validator == null)
                throw new ArgumentNullException(nameof(member.Validator),
                    "Member passed to " + nameof(MemberValidations<TS, TM>) + " component does not have a " +
                    nameof(MemberGetters.MemberValidations<TS, TM>) + ".");

            ClassSet classes = ClassSet.FromMember(Structure, Value, member,
                "Integrant.Rudiment.Component." + nameof(MemberValidations<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            if (Structure.ValidationState.IsValidating)
            {
                ValidationBuilder.RenderValidatingNotice(builder, ref seq);
            }
            else
            {
                List<Validation>? validations = Structure.ValidationState.GetMemberValidations(ID);

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