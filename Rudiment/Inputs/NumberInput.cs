using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class NumberInput<TStructure, TMember> : IInput<TStructure, TMember, int>
    {
        private readonly int?                                          _min;
        private readonly int?                                          _max;
        private readonly IInput<TStructure, TMember, int>.Transformer? _transformer;

        public NumberInput
        (
            int?                                          min         = null,
            int?                                          max         = null,
            IInput<TStructure, TMember, int>.Transformer? transformer = null
        )
        {
            if (typeof(TMember) != typeof(int) && transformer == null)
            {
                throw new ArgumentException(
                    $"Standard type of NumberInput is different from Member type '{typeof(TMember).Name}' and no transformer was provided.");
            }

            _min         = min;
            _max         = max;
            _transformer = transformer;
        }

        public event Action<TStructure, TMember>? OnInput;
        public event Action<TStructure, int>?     OnRawInput;

        public void Reset() { }

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            var classes = new ClassSet(
                "Integrant.Element.Input",
                "Integrant.Rudiment.Input." + nameof(NumberInput<TStructure, TMember>)
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

            if (_min != null)
                builder.AddAttribute(++seq, "min", _min.Value);

            if (_max != null)
                builder.AddAttribute(++seq, "max", _max.Value);

            InputBuilder.CloseInnerInput(builder);

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            string s = args.Value?.ToString() ?? "";

            int v = s == ""
                ? default
                : int.Parse(s);

            OnRawInput?.Invoke(value, v);

            if (_transformer == null)
                OnInput?.Invoke(value, (TMember) (object) v);
            else
                OnInput?.Invoke(value, _transformer.Invoke(v));
        }
    }
}