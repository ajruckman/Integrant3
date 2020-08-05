using System;
using Integrant.Element.Bits;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class CheckboxInput<TStructure> : IInput<TStructure, bool>
    {
        public event Action<TStructure, bool>? OnInput;

        private Checkbox? _checkbox;

        public void Reset()
        {
            _checkbox!.Reset();
        }

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

            _checkbox ??= new Checkbox
            (
                onToggle: c => OnInput?.Invoke(value, c),
                isChecked: () => (bool) member.InputValue.Invoke(structure, value, member),
                isDisabled: () => disabled
            );

            builder.AddContent(++seq, _checkbox.Render());

            //

            builder.CloseElement();
        };
    }
}