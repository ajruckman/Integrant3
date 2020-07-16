using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fundament.Component
{
    public class MemberValue<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Fundament.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Fundament.Value")]
        public TS Value { get; set; } = default!;

        [Parameter]
        public string? ID { get; set; }

        protected override void OnInitialized()
        {
            if (ID == null)
                throw new ArgumentNullException(nameof(ID),
                    "No ID parameter was passed to " + nameof(MemberValue<TS, TM>) + " component.");
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            List<string> classes = new List<string> {"Fundament." + nameof(MemberValue<TS, TM>)};

            Member<TS, TM> member = Structure.Get<TM>(ID!);

            if (member.MemberClasses != null)
                classes.AddRange(member.MemberClasses.Invoke(Structure, Value, member));

            bool shown = member.MemberIsVisible?.Invoke(Structure, Value, member) ?? true;

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            builder.AddContent(++seq, member.MemberFormatValue.Invoke(Structure, Value, member));

            builder.CloseElement();
        }
    }
}