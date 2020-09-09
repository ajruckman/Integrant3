using System;
using System.Collections.Generic;
using System.Linq;
using Integrant.Element.Bits;
using Integrant.Element.Layouts;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;
using Superset.Web.State;

namespace Integrant.Element.Constructs
{
    public class Header : IConstruct
    {
        public enum HeaderType
        {
            Primary,
            Secondary,
        }

        private readonly LinearLayout          _layout;
        private readonly bool                  _doHighlight;
        private readonly BitGetters.BitPixels? _maxWidth;
        private readonly string                _classes;
        private readonly UpdateTrigger?        _locationChangedTrigger;
        private          GetURL?               _highlightedURL;

        public Header
        (
            List<IBit>?           contents     = null,
            HeaderType            type         = HeaderType.Primary,
            bool                  borderTop    = false,
            bool                  borderBottom = true,
            bool                  doHighlight  = false,
            BitGetters.BitPixels? maxWidth     = null
        )
        {
            _doHighlight = doHighlight;
            _maxWidth    = maxWidth;

            if (doHighlight)
            {
                _locationChangedTrigger = new UpdateTrigger();

                if (contents != null)
                {
                    foreach (IBit content in contents)
                    {
                        if (!(content is Link link)) continue;

                        link.Spec.IsHighlighted ??= () => link.Spec.URL!.Invoke() == _highlightedURL?.Invoke();
                        // link.OnClick            +=  _onClickTrigger.Trigger;
                    }
                }
            }

            _layout = new LinearLayout(contents);

            IEnumerable<string> classes = new[]
            {
                "Integrant.Element.Construct",
                "Integrant.Element.Construct.Header",
                "Integrant.Element.Construct.Header:" + type,
            };

            if (borderTop)
                classes = classes.Append("Integrant.Element.Construct.Header:BorderTop");

            if (borderBottom)
                classes = classes.Append("Integrant.Element.Construct.Header:BorderBottom");

            _classes = string.Join(' ', classes);
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classes);

            ++seq;
            if (_maxWidth != null)
                builder.AddAttribute(seq, "style", $"max-width: {_maxWidth.Invoke()}px;");

            if (_doHighlight)
            {
                builder.OpenComponent<TriggerWrapper>(++seq);
                builder.AddAttribute(++seq, "Trigger", _locationChangedTrigger);
                builder.AddAttribute(++seq, "ChildContent",
                    new RenderFragment(b => b.AddContent(++seq, _layout.Render())));
                builder.CloseComponent();
            }
            else
            {
                builder.AddContent(++seq, _layout.Render());
            }

            builder.CloseElement();
        };

        public void Add(IBit bit)
        {
            if (bit is Link link)
            {
                if (_doHighlight)
                {
                    link.Spec.IsHighlighted ??= () => link.Spec.URL!.Invoke() == _highlightedURL?.Invoke();
                }

                _layout.Contents.Add(link);
            }
            else
            {
                _layout.Contents.Add(bit);
            }
        }

        public RenderFragment Render(string highlightedURL)
        {
            _highlightedURL = () => highlightedURL;
            return Render();
        }

        public RenderFragment Render(NavigationManager navMgr)
        {
            _highlightedURL = () => $"/{navMgr.ToBaseRelativePath(navMgr.Uri).TrimEnd('/')}";
            return Render();
        }

        public void Highlight(string url)
        {
            _highlightedURL = () => url;
        }

        private delegate string GetURL();
    }
}