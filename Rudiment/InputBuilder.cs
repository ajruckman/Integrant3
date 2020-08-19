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
            TStructure value, Member<TStructure, TMember> member, ClassSet classes
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
            TStructure value, Member<TStructure, TMember> member, ClassSet classes
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
            RenderTreeBuilder       builder,        ref int                     seq,
            TStructure              value,          Member<TStructure, TMember> member,
            string                  element,        string?                     type,
            string                  valueAttribute, object?                     v,
            bool                    required,       bool                        disabled,
            Action<ChangeEventArgs> onInput
        )
        {
            builder.OpenElement(++seq, element);
            if (type != null)
            {
                builder.AddAttribute(++seq, "type", type);
            }

            //

            ++seq;

            bool valueIsNull    = v == null;
            bool valueIsDefault = member.ConsiderDefaultNull && Equals(v, default(TMember));

            Console.Write($"{member.ID,-25} {valueIsNull,-6} {valueIsDefault,-6} ");

            if (!valueIsNull && !valueIsDefault)
            {
                Console.Write("->");
                builder.AddAttribute(++seq, valueAttribute, v);
            }

            Console.WriteLine();

            //

            builder.AddAttribute(++seq, "required", required);

            builder.AddAttribute(++seq, "disabled", disabled);

            ++seq;
            string? placeholder = member.InputPlaceholder?.Invoke(value, member);
            if (placeholder != null)
            {
                builder.AddAttribute(seq, "placeholder", placeholder);
            }

            builder.AddAttribute(++seq, "oninput", onInput);
            builder.SetUpdatesAttributeName(valueAttribute);
        }

        public static void CloseInnerInput(RenderTreeBuilder builder)
        {
            builder.CloseElement();
        }
    }
}