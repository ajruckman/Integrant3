using System;
using Integrant.Fundament;

namespace Integrant.Element
{
    public class Option<TID> : IOption<TID> where TID : IEquatable<TID>
    {
        public Option(TID value, string optionText, string? selectionText = null, bool disabled = false)
        {
            Value         = value;
            OptionText    = optionText;
            SelectionText = selectionText ?? optionText;
            Disabled      = disabled;
        }

        public TID    Value         { get; }
        public string OptionText    { get; }
        public string SelectionText { get; }
        public bool   Disabled      { get; }
    }
}