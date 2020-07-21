using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Fundament.Component
{
    public class MemberContainer<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Fundament.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Fundament.Value")]
        public TS Value { get; set; } = default!;

        [Parameter]
        public string? ID { get; set; }
        
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
                "Integrant.Fundament.Component." + nameof(MemberContainer<TS, TM>));

            bool shown = member.MemberIsVisible?.Invoke(Structure, Value, member) ?? true;

            //

            int seq = -1;
            
            builder.OpenElement(++seq, "div");
            
            builder.AddAttribute(++seq, "class", classes.ToString());
            
            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            builder.OpenComponent<CascadingValue<string>>(++seq);
            builder.AddAttribute(++seq, "Name",         "Integrant.Fundament.Member.ID");
            builder.AddAttribute(++seq, "Value",        ID);
            builder.AddAttribute(++seq, "IsFixed",      true);
            builder.AddAttribute(++seq, "ChildContent", ChildContent);
            builder.CloseComponent();
            
            builder.CloseElement();
        }
    }
}