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

        public void Reset(StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, bool> member)
        {
            _checkbox!.Reset();
        }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, bool> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(CheckboxInput<TStructure>)
            );

            bool required = InputBuilder.Required(value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            //

            _checkbox ??= new Checkbox
            (
                onToggle: c => OnInput?.Invoke(value, c),
                isChecked: () => member.Member.InputValue.Invoke(value, member.Member) is bool b && b,
                isDisabled: () => member.Member.InputIsDisabled?.Invoke(value, member.Member) == true
            );

            builder.AddContent(++seq, _checkbox.Render());

            //

            builder.CloseElement();
        };
    }
}