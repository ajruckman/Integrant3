using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class SelectInput<TStructure, TID> : IInput<TStructure, TID>
    {
        public event Action<TStructure, TID>? OnInput;

        public void Reset() { }

        public delegate TID Parser(string v);

        private readonly Parser _parser;

        public SelectInput()
        {
            if (typeof(TID) == typeof(string))
            {
                _parser = v => (TID) (object) v;
            }
            else if (typeof(TID) == typeof(int))
            {
                _parser = v => (TID) (object) int.Parse(v);
            }
            else
            {
                throw new ArgumentException(
                    $"No parser was passed to SelectInput and no fallback parser was found for type '{typeof(TID).Name}'.");
            }
        }

        public SelectInput(Parser p)
        {
            _parser = p;
        }

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TID> member
        ) => builder =>
        {
            if (member.SelectInputOptions == null)
                throw new ArgumentException(
                    "Member passed to SelectInput does not have a set SelectInputOptions getter.",
                    nameof(member.SelectInputOptions));

            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Input",
                "Integrant.Rudiment.Input." + nameof(SelectInput<TStructure, TID>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure, value, member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(structure, value, member));

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member,
                "select", null,
                "value", member.InputValue.Invoke(structure, value, member),
                required, disabled,
                args => OnChange(value, args)
            );

            foreach (IOption<TID>? option in member.SelectInputOptions.Invoke(structure, value, member))
            {
                builder.OpenElement(++seq, "option");
                builder.AddAttribute(++seq, "value", option.ID);

                builder.AddContent(++seq, option.Name);
                builder.CloseElement();
            }

            InputBuilder.CloseInnerInput(builder, ref seq);

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, _parser.Invoke(args.Value!.ToString()!));
        }
    }

    public class Option<TID> : IOption<TID>
    {
        public Option(TID id, string name, bool disabled = false)
        {
            ID       = id;
            Name     = name;
            Disabled = disabled;
        }

        public TID    ID       { get; }
        public string Name     { get; }
        public bool   Disabled { get; }
    }
}