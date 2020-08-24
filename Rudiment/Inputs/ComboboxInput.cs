using System;
using Integrant.Element.Components.Combobox;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Integrant.Rudiment.Inputs
{
    public class ComboboxInput<TStructure, TID> : IInput<TStructure, TID> where TID : IEquatable<TID>
    {
        private readonly MemberGetters.MemberSelectableInputOptions<TStructure, TID> _options;

        private Combobox<TID>? _combobox;

        public ComboboxInput(MemberGetters.MemberSelectableInputOptions<TStructure, TID> options)
        {
            _options = options;
        }

        public event Action<TStructure, TID>? OnInput;

        public void Reset
            (StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TID> member)
        {
            InitCombobox(structure, value, member);
        }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TID> member
        ) => builder =>
        {
            if (structure.JSRuntime == null)
                throw new ArgumentException(
                    "StructureInstance passed to ComboboxInput does not have a set JSRuntime.",
                    nameof(structure.JSRuntime));

            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            ClassSet classes = new ClassSet
            (
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(ComboboxInput<TStructure, TID>)
            );

            InputBuilder.Required(value, member.Member, classes);
            InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            //
            
            if (_combobox == null)
                InitCombobox(structure, value, member);
            
            builder.AddContent(++seq, _combobox!.Render());

            // builder.OpenComponent<Component>(++seq);
            // builder.AddAttribute(++seq, "Structure", structure);
            // builder.AddAttribute(++seq, "Value", value);
            // builder.AddAttribute(++seq, "Member", member);
            // builder.AddAttribute(++seq, "Options", _options);
            // builder.AddAttribute(++seq, "OnInput", EventCallback.Factory.Create<(TStructure, TID)>(this, InvokeOnClick));
            // builder.CloseComponent();
            
            object? v = member.Member.InputValue.Invoke(value, member.Member);
            
            // _combobox = new Combobox<TID>
            // (
            //     structure.JSRuntime!,
            //     () => _options.Invoke(value, member.Member),
            //     () => member.Member.InputIsDisabled?.Invoke(value, member.Member) == true,
            //     () => member.Member.InputIsRequired?.Invoke(value, member.Member) == true,
            //     member.Member.InputPlaceholder == null
            //         ? (Combobox<TID>.Placeholder?) null
            //         : () => member.Member.InputPlaceholder.Invoke(value, member.Member)
            // );
            //
            // _combobox.OnSelect += o => OnInput?.Invoke(value, o != null ? o.Value : default!);
            //
            // // _combobox.InvalidateOptions(() => _options.Invoke(value, member.Member));
            
            // TODO: Same for other getters
            _combobox!.SetOptionGetter(() => _options.Invoke(value, member.Member));

            if (v is TID vt)
            {
                _combobox.Select(vt, false);
            }
            else
            {
                _combobox.Deselect(false);
            }

            //

            // builder.AddContent(++seq, _combobox.Render());

            builder.CloseElement();
        };
        
        private class Component : ComponentBase
        {
            private Combobox<TID> _combobox = null!;

            [Parameter]
            public StructureInstance<TStructure> Structure { get; set; } = null!;

            [Parameter]
            public TStructure Value { get; set; } = default!;

            [Parameter]
            public MemberInstance<TStructure, TID> Member { get; set; } = null!;

            [Parameter]
            public MemberGetters.MemberSelectableInputOptions<TStructure, TID> Options { get; set; } = null!;
            
            [Parameter]
            public EventCallback<(TStructure, TID)> OnInput { get; set; }

            protected override void OnInitialized()
            {
                object? v = Member.Member.InputValue.Invoke(Value, Member.Member);

                _combobox = new Combobox<TID>
                (
                    Structure.JSRuntime!,
                    () => Options.Invoke(Value, Member.Member),
                    () => Member.Member.InputIsDisabled?.Invoke(Value, Member.Member) == true,
                    () => Member.Member.InputIsRequired?.Invoke(Value, Member.Member) == true,
                    Member.Member.InputPlaceholder == null
                        ? (Combobox<TID>.Placeholder?) null
                        : () => Member.Member.InputPlaceholder.Invoke(Value, Member.Member)
                );

                _combobox.OnSelect += o =>
                {
                    OnInput.InvokeAsync((Value, o != null ? o.Value : default!));
                    _combobox.SetOptionGetter(() => Options.Invoke(Value, Member.Member));
                };
            }

            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                builder.AddContent(0, _combobox.Render());
            }
        }

        private void InitCombobox
            (StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TID> member)
        {
            object? v = member.Member.InputValue.Invoke(value, member.Member);
            
            _combobox = new Combobox<TID>
            (
                structure.JSRuntime!,
                () => _options.Invoke(value, member.Member),
                () => member.Member.InputIsDisabled?.Invoke(value, member.Member) == true,
                () => member.Member.InputIsRequired?.Invoke(value, member.Member) == true,
                member.Member.InputPlaceholder == null
                    ? (Combobox<TID>.Placeholder?) null
                    : () => member.Member.InputPlaceholder.Invoke(value, member.Member)
            );
            
            _combobox.OnSelect += o =>
            {
                OnInput?.Invoke(value, o != null ? o.Value : default!);
                _combobox.SetOptionGetter(() => _options.Invoke(value, member.Member));
            };
            
            if (v is TID vt)
            {
                _combobox.Select(vt, false);
            }
            else
            {
                _combobox.Deselect(false);
            }
        }
    }
}