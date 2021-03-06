using System;

namespace Integrant.Fundament
{
    public interface IOption<TValue> where TValue : IEquatable<TValue>
    {
        /// <summary>
        /// The value by which to equate one option with another.
        /// </summary>
        // string Key { get; }

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
        /// Whether or not this option is selected.
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        /// Whether or not this option is disabled and cannot be selected.
        /// </summary>
        bool Disabled { get; set; }

        int? SerialID { get; }
    }
}