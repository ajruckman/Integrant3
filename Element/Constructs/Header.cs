using System.Collections.Generic;
using System.Linq;
using Integrant.Element.Bits;
using Integrant.Element.Layouts;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Constructs
{
    public class Header : IConstruct
    {
        private readonly bool    _doHighlight;
        private          string? _highlightedURL = null;

        public enum HeaderType
        {
            Primary,
            Secondary,
        }

        private readonly LinearLayout _layout;
        private readonly string       _classes;

        public Header
        (
            List<IBit>? contents     = null,
            HeaderType  type         = HeaderType.Primary,
            bool        borderTop    = false,
            bool        borderBottom = true,
            bool        doHighlight  = false
        )
        {
            _doHighlight = doHighlight;
            if (doHighlight && contents != null)
            {
                foreach (IBit content in contents)
                {
                    if (!(content is Link link)) continue;

                    link.Spec.IsHighlighted ??= () => link.Spec.URL!.Invoke() == _highlightedURL;
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

        public void Add(IBit bit)
        {
            if (!_doHighlight) return;
            if (!(bit is Link link)) return;

            link.Spec.IsHighlighted ??= () => link.Spec.URL!.Invoke() == _highlightedURL;
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classes);
            builder.AddContent(++seq, _layout.Render());
            builder.CloseElement();
        };

        public RenderFragment Render(string highlightedURL)
        {
            _highlightedURL = highlightedURL;
            return Render();
        }

        public RenderFragment Render(NavigationManager navMgr)
        {
            return Render($"/{navMgr.ToBaseRelativePath(navMgr.Uri).TrimEnd('/')}");
        }

        public void Highlight(string url)
        {
            _highlightedURL = url;
        }
    }
}