using System;
using System.Collections.Generic;
using System.Globalization;
using Fundament;
using Microsoft.AspNetCore.Components;

namespace Rudiment.Input
{
    public class TimeInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime> OnInput;

        public RenderFragment Render(
            Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "input");
            builder.AddAttribute(++seq, "type", "time");

            builder.AddAttribute(++seq, "oninput", new Action<ChangeEventArgs>(args => OnChange(value, args)));

            var classes = new List<string> {"Fundament.Input", "Fundament.Input." + nameof(TimeInput<TStructure>),};
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            if (member.MemberInputIsRequired?.Invoke(structure, value, member) == true)
                builder.AddAttribute(++seq, "required", "required");

            if (member.MemberInputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.MemberInputPlaceholder.Invoke(structure, value, member));

            builder.AddAttribute(++seq, "value", member.MemberDefaultValue.Invoke(structure, value, member));

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            Console.WriteLine(args.Value);
            OnInput.Invoke(value, DateTime.ParseExact(args.Value.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}