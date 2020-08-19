using System;
using Integrant.Element.Components.Combobox;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class ComboboxInput<TStructure, TID, TKey> : IInput<TStructure, TID> where TKey : IEquatable<TKey>
    {
        public delegate TID Parser(string v);

        private readonly MemberGetters.MemberSelectableInputOptions<TStructure, TID, TKey> _options;

        private Combobox<TKey, TID>? _combobox;

        // private readonly Parser _parser;

        public ComboboxInput(MemberGetters.MemberSelectableInputOptions<TStructure, TID, TKey> options)
        {
            _options = options;
            // if (typeof(TID) == typeof(string))
            // {
            //     _parser = v => (TID) (object) v;
            // }
            // else if (typeof(TID) == typeof(int))
            // {
            //     _parser = v => (TID) (object) int.Parse(v);
            // }
            // else
            // {
            //     throw new ArgumentException(
            //         $"No parser was passed to ComboboxInput and no fallback parser was found for type '{typeof(TID).Name}'.");
            // }
        }

        public event Action<TStructure, TID>? OnInput;

        public void Reset() { }

        // public ComboboxInput(Parser p)
        // {
        //     _parser = p;
        // }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TID> member
        ) => builder =>
        {
            if (structure.JSRuntime == null)
                throw new ArgumentException(
                    "StructureInstance passed to ComboboxInput does not have a set JSRuntime.",
                    nameof(structure.JSRuntime));

            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(ComboboxInput<TStructure, TID, TKey>)
            );

            // TODO: required, disabled, placeholder

            bool required = InputBuilder.Required(builder, ref seq, structure.Structure, value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure.Structure, value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            // if (member.InputPlaceholder != null)
            //     builder.AddAttribute(++seq, "placeholder",
            //         member.InputPlaceholder.Invoke(value, member.Member));

            //

            var v = (string?) member.Member.InputValue.Invoke(value, member.Member);

            if (_combobox == null)
            {
                _combobox = new Combobox<TKey, TID>
                (
                    structure.JSRuntime,
                    () => _options.Invoke(value, member.Member),
                    () => member.Member.InputIsDisabled?.Invoke(value, member.Member) == true,
                    () => member.Member.InputIsRequired?.Invoke(value, member.Member) == true,
                    member.Member.InputPlaceholder == null
                        ? (Combobox<TKey, TID>.Placeholder?) null
                        : () => member.Member.InputPlaceholder.Invoke(value, member.Member)
                );

                _combobox.OnSelect += o => OnInput?.Invoke(value, o != null ? o.Value : default!);
            }

            if (!string.IsNullOrEmpty(v))
            {
                _combobox.Select(v, false);
            }

            builder.AddContent(++seq, _combobox.Render());

            builder.CloseElement();
        };
    }
}