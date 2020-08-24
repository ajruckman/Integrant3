using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class StringInput<TStructure> : IInput<TStructure, string>
    {
        public readonly bool                  TextArea;
        public readonly bool                  Monospace;
        public readonly GetTextAreaDimension? TextAreaCols;
        public readonly GetTextAreaDimension? TextAreaRows;

        public StringInput
        (
            bool                  textArea     = false,
            bool                  monospace    = false,
            GetTextAreaDimension? textAreaCols = null,
            GetTextAreaDimension? textAreaRows = null
        )
        {
            TextArea     = textArea;
            Monospace    = monospace;
            TextAreaCols = textAreaCols;
            TextAreaRows = textAreaRows;

            if (!TextArea)
            {
                if (TextAreaCols != null)
                    throw new ArgumentException(
                        "TextAreaCols getter was passed to StringInput constructor, but this input is not a textarea.",
                        nameof(textAreaCols));
                if (TextAreaRows != null)
                    throw new ArgumentException(
                        "TextAreaRows getter was passed to StringInput constructor, but this input is not a textarea.",
                        nameof(textAreaCols));
            }
        }

        public event Action<TStructure, string>? OnInput;

        public void Reset(StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, string> member) { }

        public delegate int GetTextAreaDimension
        (
            TStructure value, Member<TStructure, string> member, IInput<TStructure, string> input
        );

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, string> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(StringInput<TStructure>)
            );

            if (TextArea) classes.Add("Integrant.Rudiment.Input:TextArea");
            if (Monospace) classes.Add("Integrant.Rudiment.Input:Monospace");

            bool required = InputBuilder.Required(value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            //

            if (!TextArea)
            {
                InputBuilder.OpenInnerInput
                (
                    builder, ref seq,
                    value, member.Member,
                    "input", "text",
                    "value", member.Member.InputValue.Invoke(value, member.Member),
                    required, disabled,
                    args => OnChange(value, args)
                );
                InputBuilder.CloseInnerInput(builder);
            }
            else
            {
                InputBuilder.OpenInnerInput
                (
                    builder, ref seq,
                    value, member.Member,
                    "textarea", null,
                    "value", member.Member.InputValue.Invoke(value, member.Member),
                    required, disabled,
                    args => OnChange(value, args)
                );
                
                if (TextAreaCols != null)
                    builder.AddAttribute(++seq, "cols", TextAreaCols.Invoke(value, member.Member, this));
                if (TextAreaRows != null)
                    builder.AddAttribute(++seq, "rows", TextAreaRows.Invoke(value, member.Member, this));
                
                InputBuilder.CloseInnerInput(builder);
            }

            builder.CloseElement();
        };

        // private string Bound
        // {
        //     get
        //     {
        //         
        //     }
        //     set
        //     {
        //         
        //     }
        // }

        private void OnChange(TStructure value, ChangeEventArgs args)
        {
            OnInput?.Invoke(value, args.Value?.ToString() ?? "");
        }
    }
}