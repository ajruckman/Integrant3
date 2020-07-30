using System.Collections.Generic;

namespace Integrant.Colorant.Schema
{
    public interface ITheme
    {
        public string              Assembly       { get; }
        public string              Name           { get; }
        public IEnumerable<string> Variants       { get; }
    }
}