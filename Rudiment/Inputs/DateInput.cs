using System;
using System.Globalization;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class DateInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime>? OnInput;

        public void Reset(StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, DateTime> member) { }

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
                "Integrant.Rudiment.Input." + nameof(DateInput<TStructure>)
            );

            bool required = InputBuilder.Required(value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                value, member.Member,
                "input", "date",
                "value", TransformValue(structure.Structure, value, member.Member),
                required, disabled,
                args => OnChange(value, args)
            );
            InputBuilder.CloseInnerInput(builder);

            builder.CloseElement();
        };

        private static string TransformValue
            (Structure<TStructure> structure, TStructure value, Member<TStructure, DateTime> member)
        {
            var v = (DateTime?) member.InputValue.Invoke(value, member);

            if (v == null) return "";

            return member.ConsiderDefaultNull
                ? v.Value == default
                    ? ""
                    : v.Value.ToString("yyyy-MM-dd")
                : v.Value.ToString("yyyy-MM-dd");
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