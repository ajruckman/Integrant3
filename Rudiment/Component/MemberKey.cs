using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Component
{
    public class MemberKey<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.StructureState")]
        public StructureState<TS> StructureState { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = StructureState.Structure.GetMember<TM>(ID);

            ClassSet classes = ClassSet.FromMember(StructureState.Structure, Value, member, 
                "Integrant.Rudiment.Component." + nameof(MemberKey<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            builder.AddContent(++seq, member.Key.Invoke(StructureState.Structure, Value, member));

            builder.CloseElement();
        }
    }
}