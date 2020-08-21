using System;
using System.Collections.Generic;
using System.Linq;
using Integrant.Element.Components.Combobox;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class ComboboxInput<TStructure, TID> : IInput<TStructure, TID> where TID : IEquatable<TID>
    {
        private readonly MemberGetters.MemberSelectableInputOptions<TStructure, TID> _options;

        private Combobox<TID>? _combobox;

        public ComboboxInput(MemberGetters.MemberSelectableInputOptions<TStructure, TID> options)
        {
            _options = options;
        }

        public event Action<TStructure, TID>? OnInput;

        public void Reset() { }

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
                "Integrant.Rudiment.Input." + nameof(ComboboxInput<TStructure, TID>)
            );

            InputBuilder.Required(value, member.Member, classes);
            InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            object? v = member.Member.InputValue.Invoke(value, member.Member);

            // if (_combobox == null)
            // {
                _combobox = new Combobox<TID>
                (
                    structure.JSRuntime,
                    () => _options.Invoke(value, member.Member),
                    () => member.Member.InputIsDisabled?.Invoke(value, member.Member) == true,
                    () => member.Member.InputIsRequired?.Invoke(value, member.Member) == true,
                    member.Member.InputPlaceholder == null
                        ? (Combobox<TID>.Placeholder?) null
                        : () => member.Member.InputPlaceholder.Invoke(value, member.Member)
                );

                _combobox.OnSelect += o => OnInput?.Invoke(value, o != null ? o.Value : default!);
            // }
            
            // _combobox.InvalidateOptions(() => _options.Invoke(value, member.Member));

            if (v is TID vt)
            {
                _combobox.Select(vt, false);
            }
            else
            {
                _combobox.Deselect(false);
            }

            builder.AddContent(++seq, _combobox.Render());

            builder.CloseElement();
        };
    }
}