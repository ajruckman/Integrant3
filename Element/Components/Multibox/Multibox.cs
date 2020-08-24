using System;
using System.Collections.Generic;
using System.Linq;
using Integrant.Element.Components.Combobox;
using Integrant.Fundament;
using Integrant.Resources.Icons.MaterialIcons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Integrant.Element.Components.Multibox
{
    public sealed class Multibox<T> where T : IEquatable<T>
    {
        private readonly Combobox<T>.OptionGetter _optionGetter;
        private readonly Combobox<T>              _combobox;

        private readonly List<IOption<T>> _selected        = new List<IOption<T>>();
        private readonly HashSet<T>       _selectedSet     = new HashSet<T>();
        private          Action           _stateHasChanged = null!;

        public event Action<List<IOption<T>>?>? OnSelect;

        public Multibox
        (
            IJSRuntime               jsRuntime,
            Combobox<T>.OptionGetter optionGetter,
            Combobox<T>.IsDisabled?  isDisabled  = null,
            Combobox<T>.IsRequired?  isRequired  = null,
            Combobox<T>.Placeholder? placeholder = null
        )
        {
            _optionGetter = optionGetter;
            _combobox = new Combobox<T>
            (
                jsRuntime,
                Options,
                isDisabled,
                isRequired,
                placeholder ?? DefaultPlaceholderGetter
            );

            _combobox.OnSelect += o => Select(o);
        }

        private IEnumerable<Option<T>> Options()
        {
            return _optionGetter.Invoke().Select(v =>
                new Option<T>(
                    v.Value,
                    v.OptionText,
                    v.SelectionText,
                    v.Disabled || _selectedSet.Contains(v.Value)
                ));
        }

        public void Select(IOption<T>? o, bool update = true)
        {
            if (o == null || _selectedSet.Contains(o.Value)) return;

            _selected.Add(o);
            _selectedSet.Add(o.Value);
            _combobox.InvalidateOptions();
            _combobox.Deselect(false);
            _stateHasChanged.Invoke();

            if (update)
                OnSelect?.Invoke(_selected);
        }
        
        public void Select(T value, bool update = true)
        {
            Select(Options().Single(v => v.Value.Equals(value)), update);
        }

        private static string DefaultPlaceholderGetter()
        {
            return "Click or tab to add selection";
        }

        private void Remove(IOption<T> option)
        {
            _selected.Remove(option);
            _selectedSet.Remove(option.Value);
            _combobox.InvalidateOptions();

            OnSelect?.Invoke(_selected.Count != 0 ? _selected : null);
        }

        public RenderFragment Render() => b =>
        {
            b.OpenComponent<Component>(0);
            b.AddAttribute(1, "Multibox", this);
            b.CloseComponent();
        };

        private sealed class Component : ComponentBase
        {
            [Parameter] public Multibox<T> Multibox { get; set; } = null!;

            protected override void OnParametersSet()
            {
                Multibox._stateHasChanged = StateHasChanged;
            }

            protected override void BuildRenderTree(RenderTreeBuilder b)
            {
                int seq = -1;

                b.OpenElement(++seq, "div");
                b.AddAttribute(++seq, "class", "Integrant.Element.Component Integrant.Element.Component.Multibox");

                // Selection

                b.OpenElement(++seq, "div");
                b.AddAttribute(++seq, "class", "Integrant.Element.Component.Multibox.Selected");
                b.OpenRegion(++seq);

                int seq2 = -1;

                foreach (var option in Multibox._selected)
                {
                    b.OpenElement(++seq2, "div");
                    b.AddContent(++seq2, option.SelectionText);

                    b.OpenComponent<Icon>(++seq2);
                    b.AddAttribute(++seq2, "ID",   "close");
                    b.AddAttribute(++seq2, "Size", (ushort) 16);
                    b.AddAttribute(++seq2, "onclick",
                        EventCallback.Factory.Create<MouseEventArgs>(this, () => OnRemoveClick(option)));
                    b.CloseComponent();

                    b.CloseElement();
                }

                b.CloseRegion();

                b.OpenElement(++seq, "span");
                b.AddAttribute(++seq, "class", "Integrant.Element.Component.Multibox.DummySelection");
                b.CloseElement();

                b.CloseElement();

                // Combobox

                b.AddContent(++seq, Multibox._combobox.Render());

                //

                b.CloseElement();
            }

            private void OnRemoveClick(IOption<T> option)
            {
                Multibox.Remove(option);
            }
        }
    }
}