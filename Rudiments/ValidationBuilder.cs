using System;
using System.Collections.Generic;
using Integrant.Fundaments;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiments
{
    public class ValidationBuilder
    {
        public static void RenderValidatingNotice(RenderTreeBuilder builder, ref int seq)
        {
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class",
                "Integrant.Rudiments.Validation.Notice Integrant.Rudiments.Validation.Notice:Validating");

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiments.Validation.Notice.Background");
            builder.CloseElement();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiments.Validation.Notice.Text");
            builder.AddContent(++seq, "Validating...");
            builder.CloseElement();

            builder.CloseElement();
        }

        public static void RenderResult(RenderTreeBuilder builder, ref int seq, Validation validation)
        {
            var classes = new List<string> {"Integrant.Rudiments.Validation.Result"};

            switch (validation.ResultType)
            {
                case ValidationResultType.Invalid:
                    classes.Add("Integrant.Rudiments.Validation.Result:Invalid");
                    break;
                case ValidationResultType.Warning:
                    classes.Add("Integrant.Rudiments.Validation.Result:Warning");
                    break;
                case ValidationResultType.Valid:
                    classes.Add("Integrant.Rudiments.Validation.Result:Valid");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiments.Validation.Result.Icon");
            switch (validation.ResultType)
            {
                case ValidationResultType.Invalid:
                    builder.AddAttribute(++seq, "data-icon", "error");
                    break;
                case ValidationResultType.Warning:
                    builder.AddAttribute(++seq, "data-icon", "warning");
                    break;
                case ValidationResultType.Valid:
                    builder.AddAttribute(++seq, "data-icon", "check");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // builder.AddAttribute(++seq, "style", "width: 7px; height: 7px;");
            builder.CloseElement();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Rudiments.Validation.Result.Text");
            builder.AddContent(++seq, validation.Message);
            builder.CloseElement();

            builder.CloseElement();
        }
    }
}