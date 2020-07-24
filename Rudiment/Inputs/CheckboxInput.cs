using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class CheckboxInput<TStructure> : IInput<TStructure, bool>
    {
        public event Action<TStructure, bool>? OnInput;

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, bool> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(CheckboxInput<TStructure>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure, value, member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member,
                "input", "checkbox",
                "checked", member.InputValue.Invoke(structure, value, member),
                required, disabled, args => OnChange(value, args)
            );
            
            InputBuilder.CloseInnerInput(builder, ref seq);

            //

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, (bool) args.Value);
        }
    }
}