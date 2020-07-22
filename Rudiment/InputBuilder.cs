using System;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Superset.Web.State;

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

        public static void OpenContainer
        (
            RenderTreeBuilder builder, ref int seq
        )
        {
            builder.OpenElement(++seq, "section");
            builder.AddAttribute(++seq, "style", "display: contents;");
        }

        public static void CloseContainer
        (
            RenderTreeBuilder builder
        )
        {
            builder.CloseElement();
        }

        public static void ProtectedInput<TStructure, TMember>
        (
            RenderTreeBuilder       builder,   ref int    seq,
            Structure<TStructure>   structure, TStructure value, Member<TStructure, TMember> member,
            string                  element,   string?    type,  string valueAttribute, UpdateTrigger resetInput,
            Action<ChangeEventArgs> onInput
        )
        {
            builder.OpenComponent<TriggerWrapper>(++seq);
            builder.AddAttribute(++seq, "Trigger", resetInput);
            builder.AddAttribute(++seq, "ChildContent", new RenderFragment(builder2 =>
            {
                int seq2 = -1;
                builder2.OpenElement(++seq2, element);
                if (type != null) builder2.AddAttribute(++seq2, "type", type);
                builder2.AddAttribute(++seq2, valueAttribute, member.DefaultValue.Invoke(structure, value, member));
                builder2.AddAttribute(++seq2, "oninput",      onInput);
                builder.SetUpdatesAttributeName(valueAttribute);
                builder2.CloseElement();
            }));
            builder.CloseComponent();
        }
    }
}