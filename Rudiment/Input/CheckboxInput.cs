using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Input
{
    public class CheckboxInput<TStructure> : IInput<TStructure, bool>
    {
        public event Action<TStructure, bool>? OnInput;

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, bool> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "input");
            builder.AddAttribute(++seq, "type", "checkbox");

            var classes = new List<string> {"Integrant.Rudiment.Input", "Integrant.Rudiment.Input." + nameof(CheckboxInput<TStructure>),};
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            if (member.MemberInputIsRequired?.Invoke(structure, value, member) == true)
                builder.AddAttribute(++seq, "required", "required");

            //

            builder.AddAttribute(++seq, "checked",
                (bool) member.MemberFormatDefaultValue.Invoke(structure, value, member));
            builder.AddAttribute(++seq, "oninput", new Action<ChangeEventArgs>(args => OnChange(value, args)));
            builder.SetUpdatesAttributeName("checked");

            //

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, (bool) args.Value);
        }
    }
}