using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Components
{
    public class MemberInput<TS, TM> : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "Integrant.Rudiment.StructureInstance")]
        public StructureInstance<TS> StructureInstance { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        private MemberInstance<TS, TM> _member = null!;

        private TM _initialValue = default!;

        protected override void OnInitialized()
        {
            _member = StructureInstance.GetMemberInstance<TM>(ID);

            if (_member.Member.Input == null)
                throw new ArgumentNullException(nameof(_member.Member.Input),
                    "MemberInput component was used on a Member with no Input.");

            _initialValue = _member.Member.Value.Invoke(Value, _member.Member);

            _member.OnRefreshInputs += RefreshInput;
            _member.OnResetInputs   += ResetInput;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ClassSet classSet = ClassSet.FromMember(Value, _member.Member,
                "Integrant.Rudiment.Component." + nameof(MemberInput<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classSet.ToString());

            builder.AddContent(++seq, _member.Input!.Render(StructureInstance, Value, _member));

            builder.CloseElement();
        }

        private void RefreshInput()
        {
            InvokeAsync(StateHasChanged);
        }

        private void ResetInput()
        {
            _member.UpdateValueImmediately(Value, _member.Member.DefaultValue == null
                ? _initialValue
                : _member.Member.DefaultValue.Invoke(Value, _member.Member));
            _member.Input!.Reset(StructureInstance, Value, _member);
        }

        public void Dispose()
        {
            Console.WriteLine(new string('-', 50) + " Disposed: " + _member.ID);
            _member.OnRefreshInputs -= RefreshInput;
            _member.OnResetInputs   -= ResetInput;
        }
    }
}