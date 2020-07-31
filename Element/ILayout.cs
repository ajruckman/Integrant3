using System.Collections.Generic;

namespace Integrant.Element
{
    public interface ILayout : IBit
    {
        public List<IBit> Contents { get; }
        public void       Add(IBit bit);
    }
}