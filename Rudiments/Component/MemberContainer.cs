using System;
using Integrant.Fundaments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiments.Component
{
    public class MemberContainer<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiments.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiments.Value")]
        public TS Value { get; set; } = default!;

        [Parameter]
        public string? ID { get; set; }

        [Parameter]
        public string? Element { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (ID == null)
                throw new ArgumentNullException(nameof(ID),
                    "No ID parameter was passed to " + nameof(MemberContainer<TS, TM>) + " component.");
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = Structure.GetMember<TM>(ID!);

            ClassSet classes = ClassSet.FromMember(Structure, Value, member,
                "Integrant.Rudiments.Component." + nameof(MemberContainer<TS, TM>));

            bool shown = member.IsVisible?.Invoke(Structure, Value, member) ?? true;

            //

            int seq = -1;
            
            builder.OpenElement(++seq, Element ?? "div");
            
            builder.AddAttribute(++seq, "class", classes.Format());
            
            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            builder.OpenComponent<CascadingValue<string>>(++seq);
            builder.AddAttribute(++seq, "Name",         "Integrant.Rudiments.Member.ID");
            builder.AddAttribute(++seq, "Value",        ID);
            builder.AddAttribute(++seq, "IsFixed",      true);
            builder.AddAttribute(++seq, "ChildContent", ChildContent);
            builder.CloseComponent();
            
            builder.CloseElement();
        }
    }
}