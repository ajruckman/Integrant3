using System;
using System.Collections.Generic;
using System.Linq;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Inputs
{
    public class SelectInput<TStructure, TID> : IInput<TStructure, TID> where TID : IEquatable<TID>
    {
        public delegate TID Parser(string v);

        private readonly MemberGetters.MemberSelectableInputOptions<TStructure, TID> _options;

        private Dictionary<int, TID>? _keyMap;

        public SelectInput(MemberGetters.MemberSelectableInputOptions<TStructure, TID> options)
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
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(SelectInput<TStructure, TID>)
            );

            bool required = InputBuilder.Required(value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            if (member.Member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.Member.InputPlaceholder.Invoke(value, member.Member));

            //

            object?            v             = member.Member.InputValue.Invoke(value, member.Member);
            List<IOption<TID>> options       = _options.Invoke(value, member.Member).ToList();
            int?               selectedIndex = null;

            void Fragment(RenderTreeBuilder b)
            {
                int iSeq = -1;

                for (var i = 0; i < options.Count; i++)
                {
                    IOption<TID> option = options[i];
                    _keyMap[i] = option.Value;

                    builder.OpenElement(++iSeq, "option");
                    builder.AddAttribute(++iSeq, "value", i);

                    ++iSeq;
                    if (option.Disabled)
                        builder.AddAttribute(iSeq, "disabled", "disabled");

                    ++iSeq;
                    if (option.Value.Equals(v))
                    {
                        builder.AddAttribute(iSeq, "selected", "selected");
                        selectedIndex = i;
                    }

                    builder.AddContent(++iSeq, option.OptionText);
                    builder.CloseElement();
                }
            }

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                value, member.Member,
                "select", null,
                "value", selectedIndex,
                required, disabled,
                args => OnChange(value, args)
            );

            _keyMap = new Dictionary<int, TID>();

            builder.OpenRegion(++seq);
            builder.AddContent(++seq, Fragment);
            builder.CloseRegion();

            if (selectedIndex == null)
            {
                builder.OpenElement(++seq, "option");
                builder.AddAttribute(++seq, "disabled", "disabled");
                builder.AddAttribute(++seq, "hidden",   "hidden");
                builder.AddAttribute(++seq, "selected", "selected");
                builder.CloseElement();
            }

            InputBuilder.CloseInnerInput(builder);

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, _keyMap![int.Parse(args.Value!.ToString()!)]);
        }
    }
}