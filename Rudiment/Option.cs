using Integrant.Fundament;

namespace Integrant.Rudiment
{
    public class Option<TID> : IOption<TID>
    {
        public Option(string key, TID value, string optionText, string? selectionText = null, bool disabled = false)
        {
            Key           = key;
            Value         = value;
            OptionText    = optionText;
            SelectionText = selectionText ?? optionText;
            Disabled      = disabled;
        }

        public string Key           { get; }
        public TID    Value         { get; }
        public string OptionText    { get; }
        public string SelectionText { get; }
        public bool   Disabled      { get; }
    }
}