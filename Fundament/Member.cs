using System;
using System.Collections.Generic;

namespace Integrant.Fundament
{
    // ReSharper disable once UnusedTypeParameter
    public interface IMember<TStructure>
    {
        public string            ID { get; }
        public List<Validation>? Validations(Structure<TStructure> structure, TStructure value);
        public event Action?     OnResetInputs;
        public void              ResetInputs();
    }

    public class Member<TStructure, TMember> : IMember<TStructure>
    {
        public Member
        (
            string                                         id,
            MemberGetters.MemberValue<TStructure, TMember> value,
            IInput<TStructure, TMember>?                   input = null,
            //
            MemberGetters.MemberKey<TStructure, TMember>?                   key                   = null,
            MemberGetters.MemberFormattedValue<TStructure, TMember>?        displayValue          = null,
            MemberGetters.MemberFormattedValue<TStructure, TMember>?        inputValue            = null,
            MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? inputMeetsRequirement = null,
            //
            MemberGetters.MemberClasses<TStructure, TMember>?          classes          = null,
            MemberGetters.MemberIsVisible<TStructure, TMember>?        isVisible        = null,
            MemberGetters.MemberInputIsEnabled<TStructure, TMember>?   inputIsEnabled   = null,
            MemberGetters.MemberInputIsRequired<TStructure, TMember>?  inputIsRequired  = null,
            MemberGetters.MemberInputPlaceholder<TStructure, TMember>? inputPlaceholder = null,
            MemberGetters.MemberValidations<TStructure, TMember>?      validator        = null,
            MemberGetters.MemberValue<TStructure, TMember>?            defaultValue     = null,
            //
            Action<TStructure, Member<TStructure, TMember>, TMember>? onValueUpdate             = null,
            int                                                       inputDebounceMilliseconds = 200
        )
        {
            ID    = id;
            Value = value;

            if (input != null)
            {
                Input         =  input;
                Input.OnInput += UpdateValue;
            }

            //

            Key          = key          ?? DefaultGetters.MemberKey;
            DisplayValue = displayValue ?? DefaultGetters.MemberFormattedValue;
            InputValue   = inputValue   ?? DefaultGetters.MemberFormattedValue;

            if (inputIsRequired != null)
            {
                InputMeetsRequirement = inputMeetsRequirement ?? DefaultGetters.MemberInputMeetsRequirement;
            }

            //

            Classes          = classes;
            IsVisible        = isVisible;
            InputIsEnabled   = inputIsEnabled;
            InputIsRequired  = inputIsRequired;
            InputPlaceholder = inputPlaceholder;
            Validator        = validator;
            DefaultValue     = defaultValue;

            //

            if (onValueUpdate != null)
                OnValueUpdate += onValueUpdate;

            _debouncer = new Utility.Debouncer<(TStructure, TMember)>(newValue =>
                OnValueUpdate?.Invoke(newValue.Item1, this, newValue.Item2), default!, inputDebounceMilliseconds);
        }

        public string ID { get; }

        public IInput<TStructure, TMember>? Input { get; }

        // Required delegates

        public readonly MemberGetters.MemberValue<TStructure, TMember> Value;

        // Required delegates with default fallback implementations

        public readonly MemberGetters.MemberKey<TStructure, TMember>                    Key;
        public readonly MemberGetters.MemberFormattedValue<TStructure, TMember>         DisplayValue;
        public readonly MemberGetters.MemberFormattedValue<TStructure, TMember>         InputValue;
        public readonly MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? InputMeetsRequirement;

        // Unrequired delegates 

        public readonly MemberGetters.MemberClasses<TStructure, TMember>?          Classes;
        public readonly MemberGetters.MemberIsVisible<TStructure, TMember>?        IsVisible;
        public readonly MemberGetters.MemberInputIsEnabled<TStructure, TMember>?   InputIsEnabled;
        public readonly MemberGetters.MemberInputIsRequired<TStructure, TMember>?  InputIsRequired;
        public readonly MemberGetters.MemberInputPlaceholder<TStructure, TMember>? InputPlaceholder;
        public readonly MemberGetters.MemberValidations<TStructure, TMember>?      Validator;
        public readonly MemberGetters.MemberValue<TStructure, TMember>?            DefaultValue;

        // // Required delegates
        //
        // public readonly MemberGetters.MemberValue<TStructure, TMember> Value;
        //
        // // Required delegates with fallback default implementations
        //
        // public readonly MemberGetters.MemberFormatKey<TStructure, TMember>              FormatKey;
        // public readonly MemberGetters.MemberFormatValue<TStructure, TMember>            FormatValue;
        // public readonly MemberGetters.MemberDefaultValue<TStructure, TMember>           DefaultValue;
        // public readonly MemberGetters.MemberFormatDefaultValue<TStructure, TMember>     FormatDefaultValue;
        // public readonly MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? InputMeetsRequirement;
        //
        // // Unrequired delegates 
        //
        // public readonly MemberGetters.MemberClasses<TStructure, TMember>?          Classes;
        // public readonly MemberGetters.MemberIsVisible<TStructure, TMember>?        IsVisible;
        // public readonly MemberGetters.MemberInputIsEnabled<TStructure, TMember>?   InputIsEnabled;
        // public readonly MemberGetters.MemberInputIsRequired<TStructure, TMember>?  InputIsRequired;
        // public readonly MemberGetters.MemberInputPlaceholder<TStructure, TMember>? InputPlaceholder;
        // public readonly MemberGetters.MemberValidations<TStructure, TMember>?      Validator;

        //

        private readonly Utility.Debouncer<(TStructure, TMember)> _debouncer;

        internal event Action? OnInput;

        public event Action<TStructure, Member<TStructure, TMember>, TMember>? OnValueUpdate;

        public void UpdateValue(TStructure value, TMember newValue)
        {
            OnInput?.Invoke();
            _debouncer.Reset((value, newValue));
        }

        public void UpdateValueImmediately(TStructure value, TMember newValue)
        {
            OnValueUpdate?.Invoke(value, this, newValue);
        }

        //

        public List<Validation>? Validations(Structure<TStructure> structure, TStructure value)
        {
            return Validator?.Invoke(structure, value, this);
        }

        //

        public event Action? OnResetInputs;

        public void ResetInputs()
        {
            OnResetInputs?.Invoke();
        }
    }
}