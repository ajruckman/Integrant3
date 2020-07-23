using System;
using System.Collections.Generic;

namespace Integrant.Fundaments
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
            MemberGetters.MemberInputIsDisabled<TStructure, TMember>?  inputIsDisabled  = null,
            MemberGetters.MemberInputIsRequired<TStructure, TMember>?  inputIsRequired  = null,
            MemberGetters.MemberInputPlaceholder<TStructure, TMember>? inputPlaceholder = null,
            MemberGetters.MemberValidations<TStructure, TMember>?      validator        = null,
            MemberGetters.MemberValue<TStructure, TMember>?            defaultValue     = null,
            //
            Action<TStructure, Member<TStructure, TMember>, TMember>? onValueUpdate             = null,
            int                                                       inputDebounceMilliseconds = 200,
            bool?                                                     considerDefaultNull       = null
        )
        {
            ID = id;

            if (input != null)
            {
                Input         =  input;
                Input.OnInput += UpdateValue;
            }

            ConsiderDefaultNull = considerDefaultNull ?? typeof(TMember) == typeof(DateTime);

            //

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

            Classes          = classes;
            IsVisible        = isVisible;
            InputIsDisabled  = inputIsDisabled;
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

        public string                       ID                  { get; }
        public IInput<TStructure, TMember>? Input               { get; }
        public bool                         ConsiderDefaultNull { get; }

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
        public readonly MemberGetters.MemberInputIsDisabled<TStructure, TMember>?  InputIsDisabled;
        public readonly MemberGetters.MemberInputIsRequired<TStructure, TMember>?  InputIsRequired;
        public readonly MemberGetters.MemberInputPlaceholder<TStructure, TMember>? InputPlaceholder;
        public readonly MemberGetters.MemberValidations<TStructure, TMember>?      Validator;
        public readonly MemberGetters.MemberValue<TStructure, TMember>?            DefaultValue;

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