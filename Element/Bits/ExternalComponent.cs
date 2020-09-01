using System.Collections.Generic;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Bits
{
    public class ExternalComponent<TComponent> : IBit where TComponent : ComponentBase
    {
        private readonly Attributes?              _parameters;
        private readonly BitGetters.BitIsVisible? _isVisible;

        public ExternalComponent
        (
            Attributes?              parameters = null,
            BitGetters.BitIsVisible? isVisible  = null
        )
        {
            _parameters = parameters;
            _isVisible  = isVisible;
        }

        public RenderFragment Render() => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "Integrant.Element.Bit Integrant.Element.Bit.ExternalComponent");

            ++seq;
            if (_isVisible?.Invoke() == false)
                builder.AddAttribute(seq, "hidden", "hidden");

            builder.OpenComponent<TComponent>(++seq);
            if (_parameters != null)
            {
                foreach (var (key, value) in _parameters)
                {
                    builder.AddAttribute(++seq, key, value);
                }
            }

            builder.CloseComponent();

            builder.CloseElement();
        };
    }

    public class Attributes : Dictionary<string, string> { }
}