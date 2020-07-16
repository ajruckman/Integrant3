using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Fundament
{
    public class Validation
    {
        public readonly ValidationResultType ResultType;
        public readonly string               Message;

        internal RenderFragment Render() => builder =>
        {
            int          seq     = -1;
            List<string> classes = new List<string> {"Validation"};

            switch (ResultType)
            {
                case ValidationResultType.Invalid:
                    classes.Add("Invalid");
                    break;
                case ValidationResultType.Warning:
                    classes.Add("Warning");
                    break;
                case ValidationResultType.Valid:
                    classes.Add("Valid");
                    break;
                // case ValidationResultType.Validating:
                // classes.Add("Validating");
                // break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            // if (Result == ValidationResultType.Validating)
            // {
            //     builder.OpenElement(seq  + 1, "span");
            //     builder.AddAttribute(seq + 2, "class", "Background");
            //     builder.CloseElement();
            // }
            // seq += 2;

            builder.OpenElement(++seq, "span");
            builder.AddAttribute(++seq, "class", "ValidationText");
            builder.AddContent(++seq, Message);
            builder.CloseElement();

            builder.CloseElement();
        };
    }

    public enum ValidationResultType
    {
        Undefined,
        Invalid,
        Warning,
        Valid,
    }
}