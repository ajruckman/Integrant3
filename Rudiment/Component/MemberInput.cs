using System;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Component
{
    public class MemberInput<TS, TM> : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "Integrant.Rudiment.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Rudiment.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Rudiment.Member.ID")]
        public string ID { get; set; } = null!;

        private Member<TS, TM> _member = null!;

        private TM _initialValue = default!;

        protected override void OnInitialized()
        {
            _member = Structure.GetMember<TM>(ID);

            if (_member.Input == null)
                throw new ArgumentNullException(nameof(_member.Input),
                    "MemberInput component was used on a Member with no Input.");

            _initialValue = _member.Value.Invoke(Structure, Value, _member);

            _member.OnResetInputs += ResetInput;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ClassSet classSet = ClassSet.FromMember(Structure, Value, _member,
                "Integrant.Rudiment.Component." + nameof(MemberInput<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classSet.Format());

            builder.AddContent(++seq, _member.Input!.Render(Structure, Value, _member));

            builder.CloseElement();
        }

        // protected override bool ShouldRender()
        // {
        //     return _canRender;
        // }

        private bool _canRender = false;

        private void ResetInput()
        {
            // _canRender = true;
            //
            // try
            // {
            //     InvokeAsync(StateHasChanged).Wait();
            // }
            // catch
            // {
            //     // ignored
            // }
            //
            // _canRender = false;

            _member.UpdateValueImmediately(Value, _member.DefaultValue == null
                ? _initialValue
                : _member.DefaultValue.Invoke(Structure, Value, _member));
        }

        public void Dispose()
        {
            Console.WriteLine(new string('-', 50) + " Disposed: " + _member.ID);
            _member.OnResetInputs -= ResetInput;
        }
    }
}