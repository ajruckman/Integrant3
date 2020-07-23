using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Component
{
    public class MemberValue<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = Structure.GetMember<TM>(ID);

            ClassSet classes = ClassSet.FromMember(Structure, Value, member,
                "Integrant.Rudiment.Component." + nameof(MemberValue<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.Format());

            object v = member.DisplayValue.Invoke(Structure, Value, member);

            builder.AddContent(++seq, member.ConsiderDefaultNull
                ? Equals(v, default(TM)) ? "" : v
                : v);

            builder.CloseElement();
        }
    }
}