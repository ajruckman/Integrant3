using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Input
{
    public class StringInput<TStructure> : IInput<TStructure, string>
    {
        public StringInput
        (
            bool textArea  = false,
            bool monospace = false
        )
        {
            TextArea  = textArea;
            Monospace = monospace;
        }

        public event Action<TStructure, string>? OnInput;

        public readonly bool TextArea;
        public readonly bool Monospace;

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, string> member
        ) => builder =>
        {
            int seq = -1;

            if (!TextArea)
            {
                builder.OpenElement(++seq, "input");
                builder.AddAttribute(++seq, "type", "text");
            }
            else
            {
                builder.OpenElement(++seq, "textarea");
            }

            //

            var classes = new List<string> {"Integrant.Rudiment.Input", "Integrant.Rudiment.Input." + nameof(StringInput<TStructure>),};
            if (TextArea) classes.Add("Integrant.Rudiment.Input:TextArea");
            if (Monospace) classes.Add("Integrant.Rudiment.Input:Monospace");
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            //

            if (member.MemberInputIsRequired?.Invoke(structure, value, member) == true)
                builder.AddAttribute(++seq, "required", "required");

            if (member.MemberInputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.MemberInputPlaceholder.Invoke(structure, value, member));

            builder.AddAttribute(++seq, "value",   member.MemberDefaultValue.Invoke(structure, value, member));
            builder.AddAttribute(++seq, "oninput", new Action<ChangeEventArgs>(args => OnChange(value, args)));
            builder.SetUpdatesAttributeName("value");

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, args.Value.ToString());
        }
    }
}