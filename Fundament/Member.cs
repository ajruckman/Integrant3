using System;
using System.Collections.Generic;
using Superset.Utilities;

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
            string                                                          id,
            MemberGetters.MemberValue<TStructure, TMember>                  value,
            IInput<TStructure, TMember>?                                    input                 = null,
            MemberGetters.MemberFormatKey<TStructure, TMember>?             formatKey             = null,
            MemberGetters.MemberFormatValue<TStructure, TMember>?           formatValue           = null,
            MemberGetters.MemberDefaultValue<TStructure, TMember>?          defaultValue          = null,
            MemberGetters.MemberFormatDefaultValue<TStructure, TMember>?    formatDefaultValue    = null,
            MemberGetters.MemberClasses<TStructure, TMember>?               classes               = null,
            MemberGetters.MemberIsVisible<TStructure, TMember>?             isVisible             = null,
            MemberGetters.MemberInputIsEnabled<TStructure, TMember>?        inputIsEnabled        = null,
            MemberGetters.MemberInputIsRequired<TStructure, TMember>?       inputIsRequired       = null,
            MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? inputMeetsRequirement = null,
            MemberGetters.MemberInputPlaceholder<TStructure, TMember>?      inputPlaceholder      = null,
            MemberGetters.MemberValidations<TStructure, TMember>?           validator             = null,
            Action<TStructure, Member<TStructure, TMember>, TMember>?       onValueUpdate         = null
        )
        {
            ID = id;

            if (input != null)
            {
                Input         =  input;
                Input.OnInput += UpdateValue;
            }

            //

            MemberValue = value;

            //

            FormatKey          = formatKey          ?? DefaultGetters.FormatMemberKey;
            FormatValue        = formatValue        ?? DefaultGetters.MemberFormatValue;
            DefaultValue       = defaultValue       ?? DefaultGetters.MemberDefaultValue;
            FormatDefaultValue = formatDefaultValue ?? DefaultGetters.MemberFormatDefaultValue;

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

            //

            if (onValueUpdate != null)
                OnValueUpdate += onValueUpdate;

            _debouncer = new Debouncer<(TStructure, TMember)>(newValue =>
                OnValueUpdate?.Invoke(newValue.Item1, this, newValue.Item2), default!, 200);
        }

        public string ID { get; }

        public IInput<TStructure, TMember>? Input { get; }

        // Required delegates

        public readonly MemberGetters.MemberValue<TStructure, TMember> MemberValue;

        // Required delegates with fallback default implementations

        public readonly MemberGetters.MemberFormatKey<TStructure, TMember>              FormatKey;
        public readonly MemberGetters.MemberFormatValue<TStructure, TMember>            FormatValue;
        public readonly MemberGetters.MemberDefaultValue<TStructure, TMember>           DefaultValue;
        public readonly MemberGetters.MemberFormatDefaultValue<TStructure, TMember>     FormatDefaultValue;
        public readonly MemberGetters.MemberInputMeetsRequirement<TStructure, TMember>? InputMeetsRequirement;

        // Unrequired delegates 

        public readonly MemberGetters.MemberClasses<TStructure, TMember>?          Classes;
        public readonly MemberGetters.MemberIsVisible<TStructure, TMember>?        IsVisible;
        public readonly MemberGetters.MemberInputIsEnabled<TStructure, TMember>?   InputIsEnabled;
        public readonly MemberGetters.MemberInputIsRequired<TStructure, TMember>?  InputIsRequired;
        public readonly MemberGetters.MemberInputPlaceholder<TStructure, TMember>? InputPlaceholder;
        public readonly MemberGetters.MemberValidations<TStructure, TMember>?      Validator;

        //

        private readonly Debouncer<(TStructure, TMember)> _debouncer;

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