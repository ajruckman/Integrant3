using Integrant.Fundaments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiments.Component
{
    public class MemberKey<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiments.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiments.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiments.Member.ID")]
        public string ID { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = Structure.GetMember<TM>(ID);

            ClassSet classes = ClassSet.FromMember(Structure, Value, member, 
                "Integrant.Rudiments.Component." + nameof(MemberKey<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            builder.AddContent(++seq, member.Key.Invoke(Structure, Value, member));

            builder.CloseElement();
        }
    }
}