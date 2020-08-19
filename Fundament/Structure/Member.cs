using System;

namespace Integrant.Fundament.Structure
{
    public interface IMember<TStructure>
    {
        public string ID { get; }

        public IMemberInstance<TStructure> Instantiate();
    }

    public sealed class Member<TStructure, TMember> : IMember<TStructure>
    {
        // Required delegates

        public readonly MemberGetters.MemberValue<TStructure, TMember> Value;

        // Required delegates with default fallback implementations

        public readonly MemberGetters.MemberKey<TStructure, TMember>                    Key;
        public readonly MemberGetters.MemberFormattedValue<TStructure, TMember>         DisplayValue;
        public readonly MemberGetters.MemberFormattedValue<TStructure, TMember>         InputValue;
        public readonly MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? InputMeetsRequirement;

        // Unrequired delegates 

        public readonly MemberGetters.MemberInput<TStructure, TMember>?           Input;
        public readonly MemberGetters.MemberClasses<TStructure, TMember>?         Classes;
        public readonly MemberGetters.MemberIsVisible<TStructure, TMember>?       IsVisible;
        public readonly MemberGetters.MemberInputIsDisabled<TStructure, TMember>? InputIsDisabled;
        public readonly MemberGetters.MemberInputIsRequired<TStructure, TMember>? InputIsRequired;

        public readonly MemberGetters.MemberInputPlaceholder<TStructure, TMember>? InputPlaceholder;

        public readonly MemberGetters.MemberInputTransformer<TStructure, TMember>?       InputTransformer;
        public readonly MemberGetters.MemberValidations<TStructure, TMember>?            Validator;
        public readonly MemberGetters.MemberValue<TStructure, TMember>?                  DefaultValue;
        // public readonly MemberGetters.MemberSelectableInputOptions<TStructure, TMember>? SelectableInputOptions;

        /// <summary>
        /// Action called to update TStructure.
        /// </summary>
        internal readonly Action<TStructure, Member<TStructure, TMember>, TMember>? ValueUpdater;

        /// <summary>
        /// Action called after TStructure is updated and revalidated.
        /// </summary>
        internal readonly Action<TStructure, Member<TStructure, TMember>, TMember>? OnValueUpdate;

        internal readonly int InputDebounceMilliseconds;

        public Member
        (
            string                                         id,
            MemberGetters.MemberValue<TStructure, TMember> value,
            //
            MemberGetters.MemberKey<TStructure, TMember>?                   key                   = null,
            MemberGetters.MemberFormattedValue<TStructure, TMember>?        displayValue          = null,
            MemberGetters.MemberFormattedValue<TStructure, TMember>?        inputValue            = null,
            MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? inputMeetsRequirement = null,
            //
            MemberGetters.MemberInput<TStructure, TMember>?                  input                  = null,
            MemberGetters.MemberClasses<TStructure, TMember>?                classes                = null,
            MemberGetters.MemberIsVisible<TStructure, TMember>?              isVisible              = null,
            MemberGetters.MemberInputIsDisabled<TStructure, TMember>?        inputIsDisabled        = null,
            MemberGetters.MemberInputIsRequired<TStructure, TMember>?        inputIsRequired        = null,
            MemberGetters.MemberInputPlaceholder<TStructure, TMember>?       inputPlaceholder       = null,
            MemberGetters.MemberInputTransformer<TStructure, TMember>?       inputTransformer       = null,
            MemberGetters.MemberValidations<TStructure, TMember>?            validator              = null,
            MemberGetters.MemberValue<TStructure, TMember>?                  defaultValue           = null,
            // MemberGetters.MemberSelectableInputOptions<TStructure, TMember>? selectableInputOptions = null,
            //
            Action<TStructure, IMember<TStructure>, TMember>? valueUpdater              = null,
            Action<TStructure, IMember<TStructure>, TMember>? onValueUpdate             = null,
            int                                               inputDebounceMilliseconds = 300,
            bool?                                             considerDefaultNull       = null
        )
        {
            if (input != null && valueUpdater == null)
            {
                throw new ArgumentException(
                    "Member was created with a MemberInput getter but no ValueUpdater to handle input values.");
            }

            ID    = id;
            Value = value;

            //

            Key          = key          ?? DefaultGetters.MemberKey;
            DisplayValue = displayValue ?? DefaultGetters.MemberFormattedValue;
            InputValue   = inputValue   ?? DefaultGetters.MemberFormattedValue;

            if (inputIsRequired != null)
            {
                InputMeetsRequirement = inputMeetsRequirement ?? DefaultGetters.MemberInputMeetsRequirement;
            }

            //

            Input                  = input;
            Classes                = classes;
            IsVisible              = isVisible;
            InputIsDisabled        = inputIsDisabled;
            InputIsRequired        = inputIsRequired;
            InputPlaceholder       = inputPlaceholder;
            InputTransformer       = inputTransformer;
            Validator              = validator;
            DefaultValue           = defaultValue;
            // SelectableInputOptions = selectableInputOptions;

            //

            ValueUpdater              = valueUpdater;
            OnValueUpdate             = onValueUpdate;
            InputDebounceMilliseconds = inputDebounceMilliseconds;
            ConsiderDefaultNull       = considerDefaultNull ?? typeof(TMember) == typeof(DateTime);
        }

        public bool ConsiderDefaultNull { get; }

        public IMemberInstance<TStructure> Instantiate() =>
            new MemberInstance<TStructure, TMember>(this);

        public string ID { get; }
    }
}