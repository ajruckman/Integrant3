using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Integrant.Rudiment.Inputs
{
    public class SelectInput<TStructure, TID> : IInput<TStructure, TID>
    {
        public event Action<TStructure, TID>? OnInput;

        public void Reset() { }

        public delegate TID Parser(string v);

        // private readonly Parser _parser;

        public SelectInput()
        {
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
            //         $"No parser was passed to SelectInput and no fallback parser was found for type '{typeof(TID).Name}'.");
            // }
        }

        // public SelectInput(Parser p)
        // {
        //     _parser = p;
        // }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TID> member
        ) => builder =>
        {
            if (member.Member.SelectableInputOptions == null)
                throw new ArgumentException(
                    "Member passed to SelectInput does not have a set SelectableInputOptions getter.",
                    nameof(member.Member.SelectableInputOptions));

            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(SelectInput<TStructure, TID>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure.Structure, value, member.Member,classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure.Structure, value, member.Member,classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            if (member.Member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.Member.InputPlaceholder.Invoke(value, member.Member));

            //

            object? v = member.Member.InputValue.Invoke(value, member.Member);

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member.Member,
                "select", null,
                "value", v,
                required, disabled,
                args => OnChange(value, args)
            );

            _keyMap = new Dictionary<string, TID>();

            var anySelected = false;

            foreach (IOption<TID>? option in member.Member.SelectableInputOptions.Invoke(value, member.Member))
            {
                _keyMap[option.Key] = option.Value;

                builder.OpenElement(++seq, "option");
                builder.AddAttribute(++seq, "value", option.Key);

                ++seq;
                if (option.Disabled)
                    builder.AddAttribute(seq, "disabled", "disabled");

                ++seq;
                if (option.Key == v?.ToString())
                {
                    builder.AddAttribute(seq, "selected", "selected");
                    anySelected = true;
                }

                builder.AddContent(++seq, option.OptionText);
                builder.CloseElement();
            }

            if (!anySelected)
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

        private Dictionary<string, TID>? _keyMap;

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, _keyMap![args.Value!.ToString()!]);
            // OnInput?.Invoke(value, _parser.Invoke(args.Value!.ToString()!));
        }
    }
}