using System;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment
{
    public static class InputBuilder
    {
        public static void Required<TStructure, TMember>
        (
            RenderTreeBuilder     builder,   ref int    seq,
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member,
            ClassSet              classes
        )
        {
            if (member.InputIsRequired?.Invoke(structure, value, member) == true)
            {
                builder.AddAttribute(++seq, "required", "required");
                classes.Add("Integrant.Rudiment.Input:Required");

                // This should always be true
                if (member.InputMeetsRequirement != null)
                {
                    classes.Add(member.InputMeetsRequirement.Invoke(structure, value, member)
                        ? "Integrant.Rudiment.Input:MeetsRequirement"
                        : "Integrant.Rudiment.Input:FailsRequirement");
                }
            }
        }

        public static void Disabled<TStructure, TMember>
        (
            RenderTreeBuilder     builder,   ref int    seq,
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member,
            ClassSet              classes
        )
        {
            if (member.InputIsDisabled?.Invoke(structure, value, member) == true)
            {
                builder.AddAttribute(++seq, "disabled", "disabled");
                classes.Add("Integrant.Rudiment.Input:Disabled");
            }
        }

        public static void Value<TStructure, TMember>
        (
            RenderTreeBuilder           builder, ref int seq,
            Member<TStructure, TMember> member,
            string                      valueAttribute, object value,
            Action<ChangeEventArgs>     onInput
        )
        {
            builder.AddAttribute(++seq, valueAttribute, member.ConsiderDefaultNull
                ? Equals(value, default(TMember)) ? "" : value
                : value
            );
            builder.AddAttribute(++seq, "oninput", onInput);
            builder.SetUpdatesAttributeName(valueAttribute);
        }
    }
}