using System;
using System.Globalization;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Input
{
    public class DateInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime>? OnInput;

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "input");
            builder.AddAttribute(++seq, "type", "date");

            //

            var classes = new ClassSet(
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(DateInput<TStructure>)
            );

            InputBuilder.Required(builder, ref seq, structure, value, member, classes);

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(structure, value, member));

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.Value(builder, ref seq, structure, value, member, "value", args => OnChange(value, args));

            builder.CloseElement();
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value,
                DateTime.ParseExact(args.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
}