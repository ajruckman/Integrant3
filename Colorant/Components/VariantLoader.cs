using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Integrant.Colorant.Schema;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Superset.Web.State;

namespace Integrant.Colorant.Components
{
    public sealed class VariantLoader
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ITheme               _theme;
        private readonly string               _defaultVariant;

        private readonly UpdateTrigger _update = new UpdateTrigger();
        private          string?       _variant;

        public VariantLoader
        (
            ILocalStorageService localStorage, ITheme theme, string defaultVariant
        )
        {
            _localStorage   = localStorage;
            _theme          = theme;
            _defaultVariant = defaultVariant;

            if (theme.Variants.Count() == 1)
            {
                _variant = theme.Variants.First();
                Complete = true;
            }
        }

        public bool Complete { get; private set; }

        public event Action? OnComplete;

        public RenderFragment RenderStylesheets()
        {
            void Fragment(RenderTreeBuilder builder)
            {
                int seq = -1;

                builder.OpenComponent<TriggerWrapper>(++seq);
                builder.AddAttribute(++seq, "Trigger", _update);

                builder.AddAttribute(++seq, "ChildContent", new RenderFragment(builder2 =>
                {
                    builder2.OpenElement(++seq, "link");
                    builder2.AddAttribute(++seq, "rel", "stylesheet");
                    builder2.AddAttribute(++seq, "href",
                        $"_content/{_theme.Assembly}/css/{_theme.Name}/{_variant}.css");
                    builder2.CloseElement();
                }));

                builder.CloseComponent();
            }

            return Fragment;
        }

        public RenderFragment RenderDropdown()
        {
            void Fragment(RenderTreeBuilder builder)
            {
                int seq = -1;

                builder.OpenElement(++seq, "div");
                builder.AddAttribute(++seq, "id", $"Integrant.Colorant.Component.{nameof(VariantLoader)}");

                builder.OpenElement(++seq, "span");

                builder.OpenElement(++seq, "label");
                builder.AddAttribute(++seq, "for", $"Integrant.Colorant.Component.{nameof(VariantLoader)}");
                builder.AddContent(++seq, "Theme");
                builder.CloseElement();

                builder.OpenComponent<TriggerWrapper>(++seq);
                builder.AddAttribute(++seq, "Trigger", _update);
                builder.AddAttribute(++seq, "ChildContent", (RenderFragment) (builder2 =>
                {
                    builder2.OpenElement(++seq, "select");
                    builder2.AddAttribute(++seq, "id",
                        $"Integrant.Colorant.Component.{nameof(VariantLoader)}.Dropdown");
                    builder2.AddAttribute(
                        ++seq,
                        "onchange",
                        EventCallback.Factory.Create<ChangeEventArgs>(this, OnVariantSelection)
                    );

                    foreach (var variant in _theme.Variants)
                    {
                        builder2.OpenElement(++seq, "option");
                        builder2.AddAttribute(++seq, "selected", _variant == variant);
                        builder2.AddContent(++seq, variant);
                        builder2.CloseElement();
                    }

                    builder2.CloseElement();
                }));
                builder.CloseComponent();

                builder.CloseElement();

                builder.CloseElement();
            }

            return Fragment;
        }

        public async Task Load()
        {
            var variant = await _localStorage.GetItemAsync<string>($"Integrant.Colorant.Variant.{_theme.Name}");
            if (string.IsNullOrEmpty(variant))
            {
                _variant = _defaultVariant;
                await _localStorage.SetItemAsync($"Integrant.Colorant.Variant.{_theme.Name}", _variant);
            }
            else
            {
                _variant = variant;
            }

            _update.Trigger();
            Complete = true;
            OnComplete?.Invoke();
        }

        private async Task OnVariantSelection(ChangeEventArgs args)
        {
            var variant = args.Value!.ToString();
            _variant = variant;
            await _localStorage.SetItemAsync($"Integrant.Colorant.Variant.{_theme.Name}", _variant);
            _update.Trigger();
        }
    }
}