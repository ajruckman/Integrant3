using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Superset.Web.State;

namespace Integrant.Fundament.Component
{
    public class MemberInput<TS, TM> : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "Integrant.Fundament.Structure")]
        public Structure<TS> Structure { get; set; } = null!;

        [CascadingParameter(Name = "Integrant.Fundament.Value")]
        public TS Value { get; set; } = default!;

        [CascadingParameter(Name = "Integrant.Fundament.Member.ID")]
        public string ID { get; set; } = null!;

        private Member<TS, TM> _member = null!;

        protected override void OnInitialized()
        {
            _member = Structure.GetMember<TM>(ID);

            if (_member.Input == null)
                throw new ArgumentNullException(nameof(_member.Input),
                    "MemberInput component was used on a Member with no Input.");

            _member.OnResetInputs += ResetInput.Trigger;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ClassSet classSet = ClassSet.FromMember(Structure, Value, _member,
                "Integrant.Fundament.Component." + nameof(MemberInput<TS, TM>));

            //

            int seq = -1;

            builder.OpenElement(++seq, "div");

            builder.AddAttribute(++seq, "class", classSet.Format());

            builder.AddContent(++seq, _member.Input!.Render(Structure, Value, _member, ResetInput));

            builder.CloseElement();
        }

        // protected override bool ShouldRender()
        // {
        //     return true;
        // }
        //
        // private bool _canRender = false;
        //
        // private void ResetInput()
        // {
        //     _canRender = true;
        //
        //     try
        //     {
        //         InvokeAsync(StateHasChanged).Wait();
        //     }
        //     catch
        //     {
        //         // ignored
        //     }
        //
        //     _canRender = false;
        //     
        //     _member.UpdateValueImmediately(Value, _member.DefaultValue.Invoke(Structure, Value, _member));
        // }

        public readonly UpdateTrigger ResetInput = new UpdateTrigger();

        public void Dispose()
        {
            Console.WriteLine(new string('-', 50) + " Disposed: " + _member.ID);
            _member.OnResetInputs -= ResetInput.Trigger;
        }
    }
}