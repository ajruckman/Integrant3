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
using Superset.Utilities;

namespace Integrant.Element.Components.Multibox
{
    public sealed class Multibox<T> where T : IEquatable<T>
    {
        private readonly Combobox<T>.OptionGetter _optionGetter;
        private readonly Combobox<T>              _combobox;

        private readonly List<IOption<T>> _selected    = new List<IOption<T>>();
        private readonly HashSet<T>       _selectedSet = new HashSet<T>();

        private readonly ThreadSafeCache<List<Option<T>>> _options;
        private          Action                           _stateHasChanged = null!;

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
            _options      = new ThreadSafeCache<List<Option<T>>>();

            _combobox = new Combobox<T>
            (
                jsRuntime,
                Options,
                isDisabled,
                isRequired,
                placeholder ?? DefaultPlaceholderGetter
            );

            _combobox.OnSelect += o => Select(o);
            
            SetSelectedOptions();
        }

        public event Action<List<IOption<T>>?>? OnSelect;

        private List<Option<T>> Options() => _options.SetIf(() =>
        {
            IOption<T>[] options = _optionGetter.Invoke().ToArray();

            List<Option<T>> result = new List<Option<T>>(options.Length);

            for (var i = 0; i < options.Length; i++)
            {
                IOption<T> v = options[i];

                result.Add(new Option<T>(
                    v.Value,
                    v.OptionText,
                    v.SelectionText,
                    v.Selected,
                    v.Disabled || _selectedSet.Contains(v.Value),
                    i
                ));
            }

            return result;
        });

        public void InvalidateOptions()
        {
            _options.Invalidate();
            SetSelectedOptions();
        }

        private void SetSelectedOptions()
        {
            _selected.Clear();
            _selectedSet.Clear();
            
            foreach (Option<T> o in Options())
            {
                if (o.Selected)
                {
                    _selected.Add(o);
                    _selectedSet.Add(o.Value);
                }
            }
        }

        public void Select(IOption<T>? o, bool update = true)
        {
            if (o == null || _selectedSet.Contains(o.Value)) return;

            o.Selected = true;
            _selected.Add(o);
            _selectedSet.Add(o.Value);
            
            // InvalidateOptions();
            _options.Invalidate();
            _combobox.InvalidateOptions();
            // _combobox.Deselect(false);
            _stateHasChanged.Invoke();

            if (update)
                OnSelect?.Invoke(_selected);
        }

        // public void Select(T value, bool update = true)
        // {
        //     Select(Options().Single(v => v.Value.Equals(value)), update);
        // }

        private static string DefaultPlaceholderGetter()
        {
            return "Click or tab to add selection";
        }

        private void Remove(IOption<T> o)
        {
            o.Selected = false;
            _selected.Remove(o);
            _selectedSet.Remove(o.Value);

            InvalidateOptions();
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

                var seq2 = 0;
                foreach (var o in Multibox._selected)
                {
                    b.OpenElement(++seq2, "div");
                    b.AddContent(++seq2, o.SelectionText);

                    b.OpenComponent<Icon>(++seq2);
                    b.AddAttribute(++seq2, "ID",   "close");
                    b.AddAttribute(++seq2, "Size", (ushort) 16);
                    b.AddAttribute(++seq2, "onclick",
                        EventCallback.Factory.Create<MouseEventArgs>(this, () => OnRemoveClick(o)));
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