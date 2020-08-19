using System;
using System.Runtime.CompilerServices;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment
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
            if (member.InputIsRequired?.Invoke(value, member) == true)
            {
                classes.Add("Integrant.Rudiment.Input:Required");

                // This should always be true
                if (member.InputMeetsRequirement != null)
                {
                    classes.Add(member.InputMeetsRequirement.Invoke(value, member)
                        ? "Integrant.Rudiment.Input:MeetsRequirement"
                        : "Integrant.Rudiment.Input:FailsRequirement");
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
            if (member.InputIsDisabled?.Invoke(value, member) == true)
            {
                classes.Add("Integrant.Rudiment.Input:Disabled");
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenInnerInput<TStructure, TMember>
        (
            RenderTreeBuilder           builder, ref int seq,
            Member<TStructure, TMember> member,
            string                      element,        string? type,
            string                      valueAttribute, object? value,
            bool                        required,       bool    disabled,
            Action<ChangeEventArgs>     onInput
        )
        {
            builder.OpenElement(++seq, element);
            if (type != null)
            {
                builder.AddAttribute(++seq, "type", type);
            }

            //

            ++seq;

            bool valueIsNull    = value == null;
            bool valueIsDefault = member.ConsiderDefaultNull && Equals(value, default(TMember));

            Console.Write($"{member.ID,-25} {valueIsNull,-6} {valueIsDefault,-6} ");

            if (!valueIsNull && !valueIsDefault)
            {
                Console.Write("->");
                builder.AddAttribute(++seq, valueAttribute, value);
            }

            Console.WriteLine();
            
            //
            
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

        public static void CloseInnerInput(RenderTreeBuilder builder)
        {
            builder.CloseElement();
        }
    }
}