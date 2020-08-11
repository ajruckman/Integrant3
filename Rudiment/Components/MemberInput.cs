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

            _initialValue = _member.Member.Value.Invoke(StructureInstance.Structure, Value, _member.Member);

            _member.OnResetInputs    += ResetInput;
            _member.OnRerenderInputs += RerenderInput;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ClassSet classSet = ClassSet.FromMember(StructureInstance.Structure, Value, _member.Member,
                "Integrant.Rudiment.Component." + nameof(MemberInput<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classSet.Format());

            builder.AddContent(++seq, _member.Input!.Render(StructureInstance.Structure, Value, _member.Member));

            builder.CloseElement();
        }

        private void ResetInput()
        {
            _member.UpdateValueImmediately(Value, _member.Member.DefaultValue == null
                ? _initialValue
                : _member.Member.DefaultValue.Invoke(StructureInstance.Structure, Value, _member.Member));
            _member.Input!.Reset();
        }

        private void RerenderInput()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Console.WriteLine(new string('-', 50) + " Disposed: " + _member.ID);
            _member.OnResetInputs    -= ResetInput;
            _member.OnRerenderInputs -= RerenderInput;
        }
    }
}