using System;
using System.Globalization;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Input
{
    public class TimeInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime>? OnInput;

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            var classes = new ClassSet(
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(TimeInput<TStructure>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure, value, member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            //
            
            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member,
                "input", "time",
                "value", TransformValue(structure, value, member),
                required, disabled,
                args => OnChange(value, args)
            );
            InputBuilder.CloseInnerInput(builder, ref seq);

            builder.CloseElement();
        };

        private static string TransformValue
            (Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member)
        {
            var v = (DateTime) member.InputValue.Invoke(structure, value, member);
            
            return member.ConsiderDefaultNull
                ? v == default ? "" : v.ToString("HH:mm:ss")
                : v.ToString("HH:mm:ss");
        }

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            string v = args.Value?.ToString() ?? "";

            OnInput?.Invoke(value, v == ""
                ? default
                : DateTime.ParseExact(v, "HH:mm:ss", CultureInfo.InvariantCulture)
            );
        }
    }
}