using System;
using System.Collections.Generic;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Constructs
{
    public class HeaderGroup : IConstruct
    {
        private const string Classes = "Integrant.Element.Construct Integrant.Element.Construct.HeaderGroup";

        private readonly BitGetters.BitPixels? _maxWidth;
        private readonly List<Header>          _headers;
        private          bool                  _hasRendered;

        public HeaderGroup
        (
            List<Header>?         headers  = null,
            BitGetters.BitPixels? maxWidth = null
        )
        {
            _maxWidth = maxWidth;
            _headers  = headers ?? new List<Header>();
        }

        public RenderFragment Render() => Render(null);

        public RenderFragment Render(NavigationManager? navMgr) => builder =>
        {
            _hasRendered = true;

            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", Classes);

            ++seq;
            if (_maxWidth != null)
                builder.AddAttribute(seq, "style", $"max-width: {_maxWidth.Invoke()}px;");

            foreach (Header header in _headers)
            {
                builder.AddContent(++seq, navMgr == null
                    ? header.Render()
                    : header.Render(navMgr)
                );
            }

            builder.CloseElement();
        };

        public void Add(Header header)
        {
            if (_hasRendered)
                throw new Exception(
                    "This HeaderGroup has already been rendered and new Headers cannot be added to it.");

            _headers.Add(header);
        }
    }
}