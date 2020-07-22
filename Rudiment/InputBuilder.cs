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

        // public static void OpenContainer
        // (
        //     RenderTreeBuilder builder, ref int seq
        // )
        // {
        //     builder.OpenElement(++seq, "section");
        //     builder.AddAttribute(++seq, "style", "display: contents;");
        // }
        //
        // public static void CloseContainer
        // (
        //     RenderTreeBuilder builder
        // )
        // {
        //     builder.CloseElement();
        // }

        // public static void ProtectedInput<TStructure, TMember>
        // (
        //     RenderTreeBuilder       builder,   ref int    seq,
        //     Structure<TStructure>   structure, TStructure value, Member<TStructure, TMember> member,
        //     string                  element,   string?    type,  string                      valueAttribute,
        //     Action<ChangeEventArgs> onInput
        // )
        // {
        //     builder.OpenElement(++seq, element);
        //     if (type != null) builder.AddAttribute(++seq, "type", type);
        //     builder.AddAttribute(++seq, valueAttribute, member.DefaultValue.Invoke(structure, value, member));
        //     builder.AddAttribute(++seq, "oninput",      onInput);
        //     builder.SetUpdatesAttributeName(valueAttribute);
        //     // builder.CloseElement();
        // }
    }
}