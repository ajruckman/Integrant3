using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Components
{
    public class MemberKey<TS, TM> : ComponentBase
    {
        [CascadingParameter(Name = "Integrant.Rudiment.StructureInstance")]
        public StructureInstance<TS> StructureInstance { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        // private readonly Restrictor _restrictor = new Restrictor();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Member<TS, TM> member = StructureInstance.Structure.GetMember<TM>(ID);

            ClassSet classes = ClassSet.FromMember(Value, member, 
                "Integrant.Rudiment.Component." + nameof(MemberKey<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classes.ToString());

            builder.AddContent(++seq, member.Key.Invoke(Value, member));

            builder.CloseElement();
        }
    }
}