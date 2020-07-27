using Integrant.Fundament;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element
{
    public abstract class Bit : IBit
    {
        public BitSpec Spec { get; protected set; }

        protected ClassSet ConstantClasses;
        
        protected string? CachedStyle;
        protected string  CachedClass;

        public abstract RenderFragment Render();

        protected string? Style(bool initial)
        {
            if (Spec.IsStatic && !initial)
                return CachedStyle;
            
            string? r = BitBuilder.StyleAttribute(Spec);

            if (Spec.IsStatic)
                CachedStyle = r;

            return r;
        }

        protected string Class(bool initial)
        {
            ClassSet c = ConstantClasses.Clone();
            
            if (Spec.IsStatic && !initial)
                return CachedClass;

            if (Spec.Classes != null)
                c.AddRange(Spec.Classes.Invoke());

            string r = c.Format();

            if (Spec.IsStatic)
                CachedClass = r;
            
            return r;
        }
    }
}