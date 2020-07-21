using System;
using System.Collections.Generic;
using System.Globalization;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Input
{
    public class DateInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime>? OnInput;

        public RenderFragment Render(
            Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "input");
            builder.AddAttribute(++seq, "type", "date");


            var classes = new List<string> {"Integrant.Rudiment.Input", "Integrant.Rudiment.Input." + nameof(DateInput<TStructure>),};
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

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
            OnInput?.Invoke(value, DateTime.ParseExact(args.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
}