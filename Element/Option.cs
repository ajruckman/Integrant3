using System;
using Integrant.Fundament;

namespace Integrant.Element
{
    public class Option<TID> : IOption<TID> where TID : IEquatable<TID>
    {
        public Option
        (
            TID     value,
            string  optionText,
            string? selectionText = null,
            bool    selected      = false,
            bool    disabled      = false,
            int?    serialID      = null
        )
        {
            Value         = value;
            OptionText    = optionText;
            SelectionText = selectionText ?? optionText;
            Selected      = selected;
            Disabled      = disabled;
            SerialID      = serialID;
        }

        public TID    Value         { get; }
        public string OptionText    { get; }
        public string SelectionText { get; }
        public bool   Selected      { get; set; }
        public bool   Disabled      { get; }

        public int? SerialID { get; }
    }
}