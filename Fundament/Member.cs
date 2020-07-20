using System;
using System.Collections.Generic;
using Superset.Utilities;

namespace Fundament
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
            string                                                     id,
            MemberGetters.MemberFormatValue<TStructure, TMember>       formatValue,
            IInput<TStructure, TMember>?                               input            = null,
            MemberGetters.MemberClasses<TStructure, TMember>?          classes          = null,
            MemberGetters.MemberIsEnabled<TStructure, TMember>?        isEnabled        = null,
            MemberGetters.MemberIsVisible<TStructure, TMember>?        isVisible        = null,
            MemberGetters.MemberFormatKey<TStructure, TMember>?        formatKey        = null,
            MemberGetters.MemberDefaultValue<TStructure, TMember>?     defaultValue     = null,
            MemberGetters.MemberFormatDefaultValue<TStructure, TMember>?     formatDefaultValue     = null,
            MemberGetters.MemberInputIsRequired<TStructure, TMember>?  inputIsRequired  = null,
            MemberGetters.MemberInputPlaceholder<TStructure, TMember>? inputPlaceholder = null,
            MemberGetters.MemberValidations<TStructure, TMember>?      validator        = null,
            Action<TStructure, Member<TStructure, TMember>, TMember>?  onValueUpdate    = null
        )
        {
            ID = id;

            if (input != null)
            {
                Input         =  input;
                Input.OnInput += UpdateValue;
            }

            MemberFormatValue = formatValue;

            MemberFormatKey          = formatKey          ?? DefaultGetters.FormatMemberKey;
            MemberDefaultValue       = defaultValue       ?? ((s, v, m) => default!);
            MemberFormatDefaultValue = formatDefaultValue ?? ((s, v, m) =>
            {
                return MemberFormatValue.Invoke(s, v, m);
            });

            MemberClasses          = classes;
            MemberIsEnabled        = isEnabled;
            MemberIsVisible        = isVisible;
            MemberInputIsRequired  = inputIsRequired;
            MemberInputPlaceholder = inputPlaceholder;
            MemberValidator        = validator;

            if (onValueUpdate != null)
                OnValueUpdate += onValueUpdate;

            //

            _debouncer = new Debouncer<(TStructure, TMember)>(newValue =>
                OnValueUpdate?.Invoke(newValue.Item1, this, newValue.Item2), default!, 200);
        }

        public string ID { get; }

        public IInput<TStructure, TMember>? Input { get; }

        // Required delegates

        public readonly MemberGetters.MemberFormatValue<TStructure, TMember> MemberFormatValue;

        // Required delegates with fallback default implementations

        public readonly MemberGetters.MemberFormatKey<TStructure, TMember>          MemberFormatKey;
        public readonly MemberGetters.MemberDefaultValue<TStructure, TMember>       MemberDefaultValue;
        public readonly MemberGetters.MemberFormatDefaultValue<TStructure, TMember> MemberFormatDefaultValue;

        // Unrequired delegates 

        public readonly MemberGetters.MemberClasses<TStructure, TMember>?          MemberClasses;
        public readonly MemberGetters.MemberIsEnabled<TStructure, TMember>?        MemberIsEnabled;
        public readonly MemberGetters.MemberIsVisible<TStructure, TMember>?        MemberIsVisible;
        public readonly MemberGetters.MemberInputIsRequired<TStructure, TMember>?  MemberInputIsRequired;
        public readonly MemberGetters.MemberInputPlaceholder<TStructure, TMember>? MemberInputPlaceholder;
        public readonly MemberGetters.MemberValidations<TStructure, TMember>?      MemberValidator;

        //

        private readonly Debouncer<(TStructure, TMember)> _debouncer;

        internal event Action? OnInput;

        public event Action<TStructure, Member<TStructure, TMember>, TMember>? OnValueUpdate;

        public void UpdateValue(TStructure value, TMember newValue)
        {
            OnInput?.Invoke();
            _debouncer.Reset((value, newValue));
        }

        //

        public List<Validation>? Validations(Structure<TStructure> structure, TStructure value)
        {
            return MemberValidator?.Invoke(structure, value, this);
        }

        //

        public event Action? OnResetInputs;

        public void ResetInputs()
        {
            OnResetInputs?.Invoke();
        }
    }
}