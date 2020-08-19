namespace Integrant.Fundament
{
    public interface IOption<TKey, TValue>
    {
        /// <summary>
        /// The value by which to equate one option with another.
        /// </summary>
        TKey Key { get; }

        /// <summary>
        /// The value which this option represents.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// The text displayed for this option in a list of options.
        /// </summary>
        string OptionText { get; }

        /// <summary>
        /// The text displayed for this option if it is selected.
        /// </summary>
        string SelectionText { get; }

        /// <summary>
        /// Whether or not this option is disabled and cannot be selected.
        /// </summary>
        bool Disabled { get; }
    }
}