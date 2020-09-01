using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Integrant.Resources.Icons;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class Icon<TIcon> : IBit
        where TIcon : IIcon
    {
        private readonly string                   _id;
        private readonly ushort                   _size;
        private readonly BitGetters.BitIsVisible? _isVisible;

        public Icon
        (
            string                   id,
            ushort                   size      = 24,
            BitGetters.BitIsVisible? isVisible = null
        )
        {
            _id        = id;
            _size      = size;
            _isVisible = isVisible;
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "section");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Bit Integrant.Element.Bit.Icon");

            ++seq;
            if (_isVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.OpenComponent<TIcon>(++seq);
            builder.AddAttribute(++seq, "ID", _id);
            builder.AddAttribute(++seq, "Size", _size);
            builder.CloseComponent();

            // builder.OpenComponent<TComponent>(++seq);
            // if (_parameters != null)
            // {
            //     foreach (var (key, value) in _parameters)
            //     {
            //         builder.AddAttribute(++seq, key, value);
            //     }
            // }
            //
            // builder.CloseComponent();

            builder.CloseElement();
        };
    }
}