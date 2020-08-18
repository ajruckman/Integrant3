using System;
using System.Collections.Generic;
using Integrant.Element.Components.Combobox;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Integrant.Rudiment.Inputs
{
    public class ComboboxInput<TStructure, TID> : IInput<TStructure, TID>
    {
        public event Action<TStructure, TID>? OnInput;

        public void Reset() { }

        private Combobox<TID>? _combobox;

        public delegate TID Parser(string v);

        // private readonly Parser _parser;

        public ComboboxInput()
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
            //         $"No parser was passed to ComboboxInput and no fallback parser was found for type '{typeof(TID).Name}'.");
            // }
        }

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
                    nameof(member.Member.SelectableInputOptions));
            if (member.Member.SelectableInputOptions == null)
                throw new ArgumentException(
                    "Member passed to ComboboxInput does not have a set SelectableInputOptions getter.",
                    nameof(member.Member.SelectableInputOptions));

            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(ComboboxInput<TStructure, TID>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure.Structure, value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure.Structure, value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            // if (member.InputPlaceholder != null)
            //     builder.AddAttribute(++seq, "placeholder",
            //         member.InputPlaceholder.Invoke(value, member.Member));

            //

            object v = member.Member.InputValue.Invoke(value, member.Member);

            if (_combobox == null)
            {
                _combobox = new Combobox<TID>
                (
                    structure.JSRuntime,
                    () => member.Member.SelectableInputOptions.Invoke(value, member.Member)
                );

                _combobox.Select(v.ToString() ?? "");
                
                _combobox.OnSelect += o => OnInput?.Invoke(value, o != null ? o.Value : default!);
            }

            builder.AddContent(++seq, _combobox.Render());

            //

            //

            // InputBuilder.OpenInnerInput
            // (
            //     builder, ref seq,
            //     member,
            //     "combobox", null,
            //     "value", v,
            //     required, disabled,
            //     args => OnChange(value, args)
            // );
            //
            // _keyMap = new Dictionary<string, TID>();
            //
            // var anyComboboxed = false;
            //
            // foreach (IOption<TID>? option in member.SelectableInputOptions.Invoke(value, member.Member))
            // {
            //     _keyMap[option.Key] = option.Value;
            //
            //     builder.OpenElement(++seq, "option");
            //     builder.AddAttribute(++seq, "value", option.Key);
            //
            //     ++seq;
            //     if (option.Disabled)
            //         builder.AddAttribute(seq, "disabled", "disabled");
            //
            //     ++seq;
            //     if (option.Key == v?.ToString())
            //     {
            //         builder.AddAttribute(seq, "selected", "comboboxed");
            //         anyComboboxed = true;
            //     }
            //
            //     builder.AddContent(++seq, option.OptionText);
            //     builder.CloseElement();
            // }
            //
            // if (!anyComboboxed)
            // {
            //     builder.OpenElement(++seq, "option");
            //     builder.AddAttribute(++seq, "disabled", "disabled");
            //     builder.AddAttribute(++seq, "hidden",   "hidden");
            //     builder.AddAttribute(++seq, "comboboxed", "comboboxed");
            //     builder.CloseElement();
            // }
            //
            // InputBuilder.CloseInnerInput(builder);

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