using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fundament.Component
{
    public class MemberValidations<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Fundament.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Fundament.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Fundament.Member.ID")]
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

            if (member.MemberValidator == null)
                throw new ArgumentNullException(nameof(member.MemberValidator),
                    "Member passed to " + nameof(MemberValidations<TS, TM>) + " component does not have a " +
                    nameof(MemberGetters.MemberValidations<TS, TM>) + ".");

            ClassSet classes = ClassSet.FromMember(Structure, Value, member,
                "Fundament.Component." + nameof(MemberValidations<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.ToString());

            if (Structure.ValidationState.IsValidating)
            {
                builder.OpenElement(++seq, "div");
                builder.AddAttribute(++seq, "class", "Fundament.Validation.Notice Fundament.Validation.Notice:Validating");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "Fundament.Validation.Notice Fundament.Validation.Notice.Background");
                builder.AddContent(++seq, "Validating...");
                builder.CloseElement();
                builder.CloseElement();
            }
            else
            {
                List<Validation>? validations = Structure.ValidationState.GetMemberValidations(ID);

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