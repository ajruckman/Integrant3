using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class SelectInput<TStructure, TID> : IInput<TStructure, TID>
    {
        public event Action<TStructure, TID>? OnInput;

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
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(SelectInput<TStructure, TID>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure, value, member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(structure, value, member));

            //

            builder.OpenElement(++seq, "select");
            
            foreach (IOption<TID>? option in member.SelectInputOptions.Invoke(structure, value, member))
            {
                builder.OpenElement(++seq, "option");
                builder.AddAttribute(++seq, "value", option.ID);

                if (member.InputValue?.Invoke(structure, value, member).Equals(option.ID) == true)
                    builder.AddAttribute(++seq, "selected", "selected");

                builder.AddContent(++seq, option.Name);
                builder.CloseElement();
            }
            
            builder.CloseElement();

            // InputBuilder.OpenInnerInput
            // (
            //     builder, ref seq,
            //     member,
            //     "input", "text",
            //     "value", member.InputValue.Invoke(structure, value, member),
            //     required, disabled,
            //     args => OnChange(value, args)
            // );
            // InputBuilder.CloseInnerInput(builder, ref seq);

            builder.CloseElement();
        };

        // private string Bound
        // {
        //     get
        //     {
        //         
        //     }
        //     set
        //     {
        //         
        //     }
        // }

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            // OnInput?.Invoke(value, args.Value?.ToString() ?? "");
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