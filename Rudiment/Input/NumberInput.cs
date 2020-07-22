using System;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Superset.Web.State;

namespace Integrant.Rudiment.Input
{
    public class NumberInput<TStructure> : IInput<TStructure, int>
    {
        public event Action<TStructure, int>? OnInput;

        public RenderFragment Render
        (
            Structure<TStructure>   structure,
            TStructure              value,
            Member<TStructure, int> member,
            UpdateTrigger           resetInput
        ) => builder =>
        {
            int seq = -1;

            InputBuilder.OpenContainer(builder, ref seq);

            //

            var classes = new ClassSet(
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(NumberInput<TStructure>)
            );

            InputBuilder.Required(builder, ref seq, structure, value, member, classes);

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(structure, value, member));

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.ProtectedInput(
                builder, ref seq, structure, value, member, "input", "number", "value",
                resetInput, args => OnChange(value, args)
            );

            //
            
            InputBuilder.CloseContainer(builder);
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, int.Parse(args.Value.ToString()));
        }
    }
}