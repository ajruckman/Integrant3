using System;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Superset.Web.State;

namespace Integrant.Rudiment.Input
{
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

        public event Action<TStructure, string>? OnInput;

        public readonly bool TextArea;
        public readonly bool Monospace;

        public RenderFragment Render
        (
            Structure<TStructure>      structure,
            TStructure                 value,
            Member<TStructure, string> member,
            UpdateTrigger              resetInput
        ) => builder =>
        {
            int seq = -1;
            
            InputBuilder.OpenContainer(builder, ref seq);


            // builder.OpenElement(++seq, "section");
            // builder.AddAttribute(++seq, "style", "display: contents;");
            //
            // if (!TextArea)
            // {
            //     builder.OpenElement(++seq, "input");
            //     builder.AddAttribute(++seq, "type", "text");
            // }
            // else
            // {
            //     builder.OpenElement(++seq, "textarea");
            // }

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Rudiment.Input",
                "Integrant.Rudiment.Input." + nameof(StringInput<TStructure>)
            );

            if (TextArea) classes.Add("Integrant.Rudiment.Input:TextArea");
            if (Monospace) classes.Add("Integrant.Rudiment.Input:Monospace");

            InputBuilder.Required(builder, ref seq, structure, value, member, classes);

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(structure, value, member));

            if (!TextArea)
            {
                InputBuilder.ProtectedInput(
                    builder, ref seq, structure, value, member, "input", "text", "value",
                    resetInput, args => OnChange(value, args)
                );
            }
            else
            {
                InputBuilder.ProtectedInput(
                    builder, ref seq, structure, value, member, "textarea", null, "value",
                    resetInput, args => OnChange(value, args)
                );
            }
            
            //

            InputBuilder.CloseContainer(builder);
        };

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, args.Value.ToString());
        }
    }
}