using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class NumberInput<TStructure> : IInput<TStructure, int>
    {
        public event Action<TStructure, int>? OnInput;

        public void Reset() { }

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, int> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            var classes = new ClassSet(
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(NumberInput<TStructure>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure, value, member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(structure, value, member));

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member,
                "input", "number",
                "value", member.InputValue.Invoke(structure, value, member),
                required, disabled,
                args => OnChange(value, args)
            );
            InputBuilder.CloseInnerInput(builder, ref seq);

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            string v = args.Value?.ToString() ?? "";

            OnInput?.Invoke(value, v == ""
                ? default
                : int.Parse(v));
        }
    }
}