using Integrant.Fundament;
using Integrant.Fundament.Element;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public abstract class BitBase : IBit
    {
        internal  BitSpec  Spec            = null!;
        protected ClassSet ConstantClasses = null!;
        protected string?  CachedStyle;

        public abstract RenderFragment Render();

        protected void Cache
        (
            string[]? additionalStyle   = null,
            string[]? additionalClasses = null
        )
        {
            if (Spec.IsStatic)
            {
                Style(true, additionalStyle);

                Class(true, additionalClasses);
            }
        }

        protected string? Style(bool initial, string[]? additional = null)
        {
            if (Spec.IsStatic && !initial)
                return CachedStyle;

            string? r = BitBuilder.StyleAttribute(Spec, additional);

            if (Spec.IsStatic)
                CachedStyle = r;

            return r;
        }

        protected string Class(bool initial, string[]? additional = null)
        {
            ClassSet c = ConstantClasses.Clone();

            if (additional != null)
                c.AddRange(additional);

            if (Spec.IsDisabled?.Invoke() == true)
                c.Add("Integrant.Element.Bit:Disabled");

            if (Spec.Classes != null)
                c.AddRange(Spec.Classes.Invoke());

            return c.ToString();
        }
    }
}