using System;
using Integrant.Fundaments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiments
{
    public static class InputBuilder
    {
        public static bool Required<TStructure, TMember>
        (
            RenderTreeBuilder     builder,   ref int    seq,
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member,
            ClassSet              classes
        )
        {
            if (member.InputIsRequired?.Invoke(structure, value, member) == true)
            {
                classes.Add("Integrant.Rudiments.Input:Required");

                // This should always be true
                if (member.InputMeetsRequirement != null)
                {
                    classes.Add(member.InputMeetsRequirement.Invoke(structure, value, member)
                        ? "Integrant.Rudiments.Input:MeetsRequirement"
                        : "Integrant.Rudiments.Input:FailsRequirement");
                }

                return true;
            }

            return false;
        }

        public static bool Disabled<TStructure, TMember>
        (
            RenderTreeBuilder     builder,   ref int    seq,
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member,
            ClassSet              classes
        )
        {
            if (member.InputIsDisabled?.Invoke(structure, value, member) == true)
            {
                classes.Add("Integrant.Rudiments.Input:Disabled");
                return true;
            }

            return false;
        }

        public static void OpenInnerInput<TStructure, TMember>
        (
            RenderTreeBuilder           builder, ref int seq,
            Member<TStructure, TMember> member,
            string                      element,        string? type,
            string                      valueAttribute, object  value,
            bool                        required,       bool    disabled,
            Action<ChangeEventArgs>     onInput
        )
        {
            builder.OpenElement(++seq, element);
            if (type != null)
            {
                builder.AddAttribute(++seq, "type", type);
            }

            builder.AddAttribute(++seq, valueAttribute, member.ConsiderDefaultNull
                ? Equals(value, default(TMember)) ? "" : value
                : value
            );

            if (required)
            {
                builder.AddAttribute(++seq, "required", "required");
            }

            if (disabled)
            {
                builder.AddAttribute(++seq, "disabled", "disabled");
            }

            builder.AddAttribute(++seq, "oninput", onInput);
            builder.SetUpdatesAttributeName(valueAttribute);
            
            // builder.CloseElement();
        }

        public static void CloseInnerInput(RenderTreeBuilder builder, ref int seq)
        {
            builder.CloseElement();
        }
    }
}