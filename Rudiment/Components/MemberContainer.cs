using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Components
{
    public class MemberContainer<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.StructureInstance")]
        public StructureInstance<TS> StructureInstance { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [Parameter]
        public string? ID { get; set; }

        [Parameter]
        public string? Element { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        protected override void OnInitialized()
        {
            if (ID == null)
                throw new ArgumentNullException(nameof(ID),
                    "No ID parameter was passed to " + nameof(MemberContainer<TS, TM>) + " component.");
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = StructureInstance.Structure.GetMember<TM>(ID!);

            ClassSet classes = ClassSet.FromMember(Value, member,
                "Integrant.Rudiment.Component." + nameof(MemberContainer<TS, TM>));

            bool shown = member.IsVisible?.Invoke(Value, member) ?? true;

            //

            int seq = -1;
            
            builder.OpenElement(++seq, Element ?? "div");
            
            builder.AddAttribute(++seq, "class", classes.ToString());
            
            if (!shown)
                builder.AddAttribute(++seq, "hidden", "hidden");

            builder.OpenComponent<CascadingValue<string>>(++seq);
            builder.AddAttribute(++seq, "Name",         "Integrant.Rudiment.Member.ID");
            builder.AddAttribute(++seq, "Value",        ID);
            builder.AddAttribute(++seq, "IsFixed",      true);
            builder.AddAttribute(++seq, "ChildContent", ChildContent);
            builder.CloseComponent();
            
            builder.CloseElement();
        }
    }
}