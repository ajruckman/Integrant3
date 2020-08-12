using System;
using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Integrant.Resources.Icons.MaterialIcons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment
{
    public class ValidationBuilder
    {
        public static void RenderValidatingNotice(RenderTreeBuilder builder, ref int seq)
        {
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",
                "Integrant.Rudiment.Validation.Notice Integrant.Rudiment.Validation.Notice:Validating");

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiment.Validation.Notice.Background");
            builder.CloseElement();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiment.Validation.Notice.Text");
            builder.AddContent(++seq, "Validating...");
            builder.CloseElement();

            builder.CloseElement();
        }

        public static RenderFragment RenderResult(Validation validation) => builder =>
        {
            int seq = -1;

            var classes = new List<string> {"Integrant.Rudiment.Validation.Result"};

            switch (validation.ResultType)
            {
                case ValidationResultType.Invalid:
                    classes.Add("Integrant.Rudiment.Validation.Result:Invalid");
                    break;
                case ValidationResultType.Warning:
                    classes.Add("Integrant.Rudiment.Validation.Result:Warning");
                    break;
                case ValidationResultType.Valid:
                    classes.Add("Integrant.Rudiment.Validation.Result:Valid");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            // builder.OpenElement(++seq, "div");
            // builder.AddAttribute(++seq, "class", "Integrant.Rudiment.Validation.Result.Icon");
            // switch (validation.ResultType)
            // {
            //     case ValidationResultType.Invalid:
            //         builder.AddAttribute(++seq, "data-icon", "error");
            //         break;
            //     case ValidationResultType.Warning:
            //         builder.AddAttribute(++seq, "data-icon", "warning");
            //         break;
            //     case ValidationResultType.Valid:
            //         builder.AddAttribute(++seq, "data-icon", "check");
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
            //
            // // builder.AddAttribute(++seq, "style", "width: 7px; height: 7px;");
            // builder.CloseElement();

            builder.OpenComponent<Icon>(++seq);

            switch (validation.ResultType)
            {
                case ValidationResultType.Invalid:
                    builder.AddAttribute(++seq, "ID", "error");
                    break;
                case ValidationResultType.Warning:
                    builder.AddAttribute(++seq, "ID", "warning");
                    break;
                case ValidationResultType.Valid:
                    builder.AddAttribute(++seq, "ID", "check");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.CloseComponent();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiment.Validation.Result.Text");
            builder.AddContent(++seq, validation.Message);
            builder.CloseElement();

            builder.CloseElement();
        };
    }
}