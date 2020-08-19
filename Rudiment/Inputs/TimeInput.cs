using System;
using System.Globalization;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class TimeInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime>? OnInput;

        public void Reset() { }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, DateTime> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            var classes = new ClassSet(
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(TimeInput<TStructure>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure.Structure, value, member.Member,classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure.Structure, value, member.Member,classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member.Member,
                "input", "time",
                "value", TransformValue(value, member.Member),
                required, disabled,
                args => OnChange(value, args)
            );
            InputBuilder.CloseInnerInput(builder);

            builder.CloseElement();
        };

        private static string TransformValue(TStructure value, Member<TStructure, DateTime> member)
        {
            var v = (DateTime?) member.InputValue.Invoke(value, member);

            if (v == null) return "";

            return member.ConsiderDefaultNull
                ? v.Value == default
                    ? ""
                    : v.Value.ToString("HH:mm:ss")
                : v.Value.ToString("HH:mm:ss");
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