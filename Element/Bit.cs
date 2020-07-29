using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public abstract class Bit : IBit
    {
        internal BitSpec Spec { get; set; }

        protected ClassSet ConstantClasses;

        protected string? CachedStyle;
        protected string  CachedClass;

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

                Class(true);
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

            if (Spec.IsStatic && !initial)
                return CachedClass;

            if (Spec.Classes != null)
                c.AddRange(Spec.Classes.Invoke());

            if (additional != null)
                c.AddRange(additional);

            string r = c.Format();

            if (Spec.IsStatic)
                CachedClass = r;

            return r;
        }
    }
}