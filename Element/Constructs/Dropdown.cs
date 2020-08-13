using System.Collections.Generic;
using System.Linq;
using Integrant.Element.Layouts;
using Integrant.Fundament.Element;
using Integrant.Resources.Icons.MaterialIcons;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Constructs
{
    public class Dropdown : IConstruct
    {
        private readonly IBit       _top;
        private readonly List<IBit> _contents;
        private readonly bool       _left;
        private readonly string     _classes;

        public Dropdown
        (
            IBit        top,
            List<IBit>? contents = null,
            bool        left     = false
        )
        {
            _top      = top;
            _contents = contents ?? new List<IBit>();
            _left     = left;

            IEnumerable<string> classes = new[]
            {
                "Integrant.Element.Construct",
                "Integrant.Element.Construct.Dropdown",
            };

            if (_left)
                classes = classes.Append("Integrant.Element.Construct.Dropdown:Left");

            _classes = string.Join(' ', classes);
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", _classes);

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Construct.Dropdown.Top");
            builder.AddContent(++seq, _top.Render());
            builder.OpenComponent<Icon>(++seq);
            builder.AddAttribute(++seq, "ID",   !_left ? "expand_more" : "chevron_right");
            builder.AddAttribute(++seq, "Size", (ushort) 24);
            builder.CloseComponent();
            builder.CloseElement();

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Construct.Dropdown.Contents");

            foreach (var bit in _contents)
            {
                builder.OpenElement(++seq, "div");
                builder.AddAttribute(++seq, "class", "Integrant.Element.Construct.Dropdown.Child");
                builder.AddContent(++seq, bit.Render());
                builder.CloseElement();
            }

            // builder.AddContent(++seq, _layout.Render());
            builder.CloseElement();

            builder.CloseElement();
        };

        public void Add(IBit bit) => _contents.Add(bit);
    }
}