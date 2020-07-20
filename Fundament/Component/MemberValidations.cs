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

        [Parameter]
        public string? ID { get; set; }

        protected override void OnInitialized()
        {
            if (ID == null)
                throw new ArgumentNullException(nameof(ID),
                    "No ID parameter was passed to " + nameof(MemberValidations<TS, TM>) + " component.");

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
            List<string> classes = new List<string> {"Fundament.Component." + nameof(MemberValidations<TS, TM>)};

            Member<TS, TM> member = Structure.GetMember<TM>(ID!);

            if (member.MemberValidator == null)
                throw new ArgumentNullException(nameof(member.MemberValidator),
                    "Member passed to " + nameof(MemberValidations<TS, TM>) + " component does not have a " +
                    nameof(Getters.MemberValidations<TS, TM>) + ".");

            if (member.MemberClasses != null)
                classes.AddRange(member.MemberClasses.Invoke(Structure, Value, member));

            bool shown = member.MemberIsVisible?.Invoke(Structure, Value, member) ?? true;

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
                List<Validation>? validations = Structure.ValidationState.GetMemberValidations(ID!);

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