using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Fundament.Input
{
    public interface IInput<TStructure, TMember>
    {
        public event Action<TMember> OnInput;

        public RenderFragment Render(
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        );
    }

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

        public readonly bool TextArea;
        public readonly bool Monospace;

        public RenderFragment Render(
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

            builder.AddAttribute(++seq, "oninput", new Action<ChangeEventArgs>(OnChange));

            //

            var classes = new List<string> {"Fundament.Input", "Fundament.Input." + nameof(StringInput<TStructure>),};
            if (TextArea) classes.Add("Fundament.Input:TextArea");
            if (Monospace) classes.Add("Fundament.Input:Monospace");
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            //

            if (member.MemberInputIsRequired != null)
                if (member.MemberInputIsRequired.Invoke(structure, value, member))
                    builder.AddAttribute(++seq, "required", "required");

            if (member.MemberInputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.MemberInputPlaceholder.Invoke(structure, value, member));

            //

            if (!TextArea)
                builder.AddAttribute(++seq, "value", member.MemberDefaultValue.Invoke(structure, value, member));
            else
                builder.AddContent(++seq, member.MemberDefaultValue.Invoke(structure, value, member));

            //

            builder.CloseElement();
        };

        private void OnChange(ChangeEventArgs obj)
        {
            OnInput?.Invoke(obj.Value.ToString());
        }

        public event Action<string>? OnInput;
    }
}