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
            InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.Value
            (
                builder, ref seq,
                member,
                "value", TransformValue(structure, value, member),
                args => OnChange(value, args)
            );

            builder.CloseElement();
        };

        private static string TransformValue
            (Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member)
        {
            var v = (DateTime) member.InputValue.Invoke(structure, value, member);

            return member.ConsiderDefaultNull
                ? v == default ? "" : v.ToString("yyyy-MM-dd")
                : v.ToString("yyyy-MM-dd");
        }

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            string v = args.Value?.ToString() ?? "";

            OnInput?.Invoke(value, v == ""
                ? default
                : DateTime.ParseExact(v, "yyyy-MM-dd", CultureInfo.InvariantCulture)
            );
        }
    }
}