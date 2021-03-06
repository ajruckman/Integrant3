using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Integrant.Fundament;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Superset.Utilities;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace Integrant.Element.Components.Combobox
{
    public sealed class Combobox<T> where T : IEquatable<T>
    {
        public delegate IEnumerable<IOption<T>> OptionGetter();

        public delegate bool IsDisabled();

        public delegate bool IsRequired();

        public delegate string Placeholder();

        private readonly IJSRuntime                    _jsRuntime;
        private readonly IsDisabled?                   _isDisabled;
        private readonly IsRequired?                   _isRequired;
        private readonly Placeholder?                  _placeholder;
        private readonly ThreadSafeCache<IOption<T>[]> _options         = new ThreadSafeCache<IOption<T>[]>();
        private readonly ThreadSafeCache<IOption<T>[]> _optionsFiltered = new ThreadSafeCache<IOption<T>[]>();
        private          OptionGetter                  _optionGetter;
        private          ElementReference              _elementRef;

        private bool _shown;
        private bool _justSelected;

        private string? _searchTerm;

        private IOption<T>? _selected;
        private IOption<T>? _focused;
        private Action      _stateHasChanged = null!;
        private bool        _shouldRender    = true;

        private readonly Debouncer<string?> _debouncer;

        public Combobox
        (
            IJSRuntime   jsRuntime,
            OptionGetter optionGetter,
            IsDisabled?  isDisabled  = null,
            IsRequired?  isRequired  = null,
            Placeholder? placeholder = null
        )
        {
            _jsRuntime    = jsRuntime;
            _optionGetter = optionGetter;
            _isDisabled   = isDisabled;
            _isRequired   = isRequired;
            _placeholder  = placeholder;

            _debouncer = new Debouncer<string?>(s =>
            {
                SetSearchTerm(s);
                Show();
                Deselect();

                // TODO: keep this?
                _focused = null;

                _shouldRender = true;
                _stateHasChanged.Invoke();
            }, null);

            // Cache this so we find any selected option.
            Options();
        }

        public event Action<IOption<T>?>? OnSelect;
        public event Action<IOption<T>?>? OnFocus;
        public event Action<string?>?     OnSetSearchTerm;
        public event Action?              OnShow;
        public event Action?              OnHide;

        //

        public void SetOptionGetter(OptionGetter optionGetter)
        {
            _optionGetter = optionGetter;
            InvalidateOptions();
        }

        public void InvalidateOptions()
        {
            _optionsFiltered.Invalidate();
            _options.Invalidate();

            if (_focused == null) return;
            IOption<T>? newFocused = Options().SingleOrDefault(v => v.Value.Equals(_focused.Value));
            _focused = newFocused ?? null;
        }

        private string InputValue()
        {
            return _shown
                ? _focused?.SelectionText  ?? _selected?.SelectionText ?? _searchTerm ?? ""
                : _selected?.SelectionText ?? _searchTerm              ?? "";
        }

        private IOption<T>[] Options() =>
            _options.SetIf(() =>
            {
                IOption<T>[] r = _optionGetter.Invoke().ToArray();

                IOption<T>? selected = r.FirstOrDefault(v => v.Selected);
                if (selected != null)
                    _selected = selected;

                return r;
            });

        private IOption<T>[] OptionsFiltered()
        {
            if (_searchTerm == null) return Options().ToArray();

            return _optionsFiltered.SetIf(() => Options().Where(Matches).ToArray());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Matches(IOption<T> o)
        {
            return _searchTerm == null || o.OptionText.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase);
        }

        public void Show()
        {
            _shown = true;
            OnShow?.Invoke();
        }

        public void Hide()
        {
            _shown = false;
            OnHide?.Invoke();
        }

        public void SetSearchTerm(string? term)
        {
            _searchTerm = term;
            _optionsFiltered.Invalidate();
            OnSetSearchTerm?.Invoke(term);
        }

        public void Select(IOption<T> o, bool update = true, bool focus = true)
        {
            if (_selected != null) _selected.Selected = false;

            _selected = o;

            SetSearchTerm(null);
            _justSelected = true;

            if (focus)
            {
                Focus(o);
            }

            if (update)
            {
                Hide();
                OnSelect?.Invoke(o);
            }
        }

        public void Deselect(bool update = true)
        {
            _selected = null;

            if (update)
                OnSelect?.Invoke(null);
        }

        private void Focus(IOption<T> o)
        {
            _focused = o;
            OnFocus?.Invoke(o);
        }

        //

        public RenderFragment Render() => b =>
        {
            b.OpenComponent<Component>(0);
            b.AddAttribute(1, "Combobox", this);
            b.CloseComponent();
        };

        //

        private void OnInputClick(MouseEventArgs args)
        {
            if (args.Button           != 0) return;
            if (_isDisabled?.Invoke() == true) return;
            Show();
        }

        private void OnInputFocus(FocusEventArgs args)
        {
            if (_isDisabled?.Invoke() == true) return;
            Show();
        }

        private void OnInputBlur(FocusEventArgs args)
        {
            Hide();
        }

        private void OnInputKeyDown(KeyboardEventArgs args)
        {
            if ((args.Key == "ArrowUp" || args.Key == "ArrowDown") && !_shown)
            {
                Show();
            }

            IOption<T>[] users = OptionsFiltered();

            IOption<T>? first = users.FirstOrDefault();
            if (first == null)
            {
                return;
            }

            switch (args.Key)
            {
                case "ArrowUp":
                    if (_focused == null)
                    {
                        Focus(first);
                    }
                    else if (!_focused.Value.Equals(first.Value))
                    {
                        int previousIndex = Array.FindIndex(users, v => v.Value.Equals(_focused.Value)) - 1;
                        // int previousIndex = users.FindIndex(v => v.Value.Equals(_focused.Value)) - 1;
                        Focus(users[previousIndex]);
                    }

                    break;

                case "ArrowDown":
                    if (_focused == null)
                    {
                        Focus(first);
                    }
                    else
                    {
                        int nextIndex = Array.FindIndex(users, v => v.Value.Equals(_focused.Value)) + 1;
                        // int nextIndex = users.FindIndex(v => v.Value.Equals(_focused.Value)) + 1;
                        if (nextIndex < users.Length)
                        {
                            Focus(users[nextIndex]);
                        }
                    }

                    break;

                case "Enter":
                    if (_shown && _focused?.Disabled == false)
                    {
                        Select(_focused);
                    }

                    break;

                case "Escape":
                    Hide();

                    break;
            }
        }

        // private void OnInputKeyUp(KeyboardEventArgs args) { }

        private void OnInputInput(ChangeEventArgs args)
        {
            var v = args.Value?.ToString();
            _shouldRender = false;
            _debouncer.Reset(string.IsNullOrEmpty(v) ? null : v);
        }

        //

        // This is required for 'onmousedown:preventDefault' to work.
        private void OnOptionMouseDown(MouseEventArgs args) { }

        // ReSharper disable once UnusedParameter.Local
        private void OnOptionClick(MouseEventArgs _, IOption<T> o)
        {
            if (!o.Disabled)
            {
                Select(o);
            }
        }

        //

        [JSInvokable]
        public void SelectFromJS(string i)
        {
            Select(Options()[int.Parse(i)]);
        }

        private sealed class Component : ComponentBase
        {
            [Parameter]
            public Combobox<T> Combobox { get; set; } = null!;

            protected override void OnParametersSet()
            {
                Combobox._stateHasChanged = () => InvokeAsync(StateHasChanged);
            }

            protected override void OnAfterRender(bool firstRender)
            {
                if (firstRender)
                {
                    Combobox._jsRuntime.InvokeVoidAsync
                    (
                        "Integrant.Element.CreateCombobox",
                        Combobox._elementRef
                    );
                }

                if (Combobox._shown) { }

                if (Combobox._justSelected && Combobox._shown)
                {
                    Combobox._justSelected = false;
                    if (Combobox._selected != null)
                    {
                        Combobox._jsRuntime.InvokeVoidAsync("Integrant.Element.ScrollDropdownToSelection",
                            Combobox._elementRef);
                    }
                }
            }

            protected override bool ShouldRender() => Combobox._shouldRender;

            protected override void BuildRenderTree(RenderTreeBuilder b)
            {
                int seq = -1;

                b.OpenElement(++seq, "div");
                b.AddAttribute(++seq, "class", "Integrant.Element.Component Integrant.Element.Component.Combobox");
                b.AddElementReferenceCapture(++seq, elemRef => Combobox._elementRef = elemRef);

                // Input

                b.OpenElement(++seq, "div");

                var classes = new ClassSet(
                    "Integrant.Element.Component.Combobox.Input",
                    "Integrant.Element.Override.Input"
                );

                if (Combobox._isDisabled?.Invoke() == true)
                    classes.Add("Integrant.Element.Override.Input:Disabled");

                if (Combobox._isRequired?.Invoke() == true && Combobox._selected == null)
                    classes.Add("Integrant.Element.Override.Input:FailsRequirement");

                b.AddAttribute(++seq, "class", classes.ToString());

                b.OpenElement(++seq, "input");
                b.AddAttribute(++seq, "type",               "text");
                b.AddAttribute(++seq, "value",              Combobox.InputValue());
                b.AddAttribute(++seq, "data-has-selection", Combobox._selected             != null);
                b.AddAttribute(++seq, "disabled",           Combobox._isDisabled?.Invoke() == true);
                b.AddAttribute(++seq, "required",           Combobox._isRequired?.Invoke() == true);

                ++seq;
                if (Combobox._placeholder != null)
                    b.AddAttribute(seq, "placeholder", Combobox._placeholder.Invoke());

                b.AddAttribute(++seq, "onclick",
                    EventCallback.Factory.Create<MouseEventArgs>(this, Combobox.OnInputClick));
                b.AddAttribute(++seq, "onfocus",
                    EventCallback.Factory.Create<FocusEventArgs>(this, Combobox.OnInputFocus));
                b.AddAttribute(++seq, "onblur",
                    EventCallback.Factory.Create<FocusEventArgs>(this, Combobox.OnInputBlur));
                b.AddAttribute(++seq, "onkeydown",
                    EventCallback.Factory.Create<KeyboardEventArgs>(this, Combobox.OnInputKeyDown));
                b.AddAttribute(++seq, "oninput",
                    EventCallback.Factory.Create<ChangeEventArgs>(this, Combobox.OnInputInput));

                b.CloseElement();
                b.CloseElement();

                // Dropdown

                b.OpenElement(++seq, "div");
                b.AddAttribute(++seq, "class",      "Integrant.Element.Component.Combobox.Dropdown");
                b.AddAttribute(++seq, "data-shown", Combobox._shown);

                b.OpenRegion(++seq);

                int seq2 = -1;

                for (var i = 0; i < Combobox.Options().Length; i++)
                {
                    IOption<T> o        = Combobox.Options()[i];
                    bool       selected = Combobox._selected?.Value.Equals(o.Value) == true;
                    bool       focused  = Combobox._focused?.Value.Equals(o.Value)  == true;
                    bool       matches  = Combobox.Matches(o);

                    b.OpenElement(++seq2, "div");
                    b.AddAttribute(++seq,  "class",         "Integrant.Element.Component.Combobox.Option");
                    b.AddAttribute(++seq2, "hidden",        !matches);
                    b.AddAttribute(++seq2, "data-i",        i);
                    b.AddAttribute(++seq2, "data-focused",  focused);
                    b.AddAttribute(++seq2, "data-selected", selected);
                    b.AddAttribute(++seq2, "data-disabled", o.Disabled);

                    b.AddAttribute(++seq2, "onmousedown",
                        EventCallback.Factory.Create<MouseEventArgs>(this, Combobox.OnOptionMouseDown));
                    b.AddEventPreventDefaultAttribute(++seq2, "onmousedown", true);

                    b.AddAttribute(++seq2, "onclick",
                        EventCallback.Factory.Create<MouseEventArgs>(this,
                            args => Combobox.OnOptionClick(args, o)));

                    b.AddContent(++seq2, o.OptionText);

                    b.CloseElement();
                }

                b.CloseRegion();

                b.CloseElement();

                //

                b.CloseElement();
            }
        }
    }
}