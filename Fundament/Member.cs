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
    }

    public class Member<TStructure, TMember> : IMember<TStructure>
    {
        public Member
        (
            string                                                    id,
            Getters.MemberFormatValue<TStructure, TMember>            memberFormatValue,
            IInput<TStructure, TMember>?                              input                  = null,
            Getters.MemberClasses<TStructure, TMember>?               memberClasses          = null,
            Getters.MemberIsEnabled<TStructure, TMember>?             memberIsEnabled        = null,
            Getters.MemberIsVisible<TStructure, TMember>?             memberIsVisible        = null,
            Getters.MemberFormatKey<TStructure, TMember>?             memberFormatKey        = null,
            Getters.MemberDefaultValue<TStructure, TMember>?          memberDefaultValue     = null,
            Getters.MemberInputIsRequired<TStructure, TMember>?       memberInputIsRequired  = null,
            Getters.MemberInputPlaceholder<TStructure, TMember>?      memberInputPlaceholder = null,
            Getters.MemberValidations<TStructure, TMember>?           memberValidator        = null,
            Action<TStructure, Member<TStructure, TMember>, TMember>? onValueUpdate          = null
        )
        {
            ID = id;

            if (input != null)
            {
                Input         =  input;
                Input.OnInput += UpdateValue;
            }

            MemberFormatValue = memberFormatValue;

            MemberFormatKey    = memberFormatKey    ?? DefaultGetters.FormatMemberKey;
            MemberDefaultValue = memberDefaultValue ?? ((s, v, m) => MemberFormatValue.Invoke(s, v, m));

            MemberClasses          = memberClasses;
            MemberIsEnabled        = memberIsEnabled;
            MemberIsVisible        = memberIsVisible;
            MemberInputIsRequired  = memberInputIsRequired;
            MemberInputPlaceholder = memberInputPlaceholder;
            MemberValidator        = memberValidator;

            if (onValueUpdate != null)
                OnValueUpdate += onValueUpdate;

            //

            _debouncer = new Debouncer<(TStructure, TMember)>(newValue =>
                OnValueUpdate?.Invoke(newValue.Item1, this, newValue.Item2), default!, 200);
        }

        public string ID { get; }

        public IInput<TStructure, TMember>? Input { get; }

        // Required delegates

        public readonly Getters.MemberFormatValue<TStructure, TMember> MemberFormatValue;

        // Required delegates with fallback default implementations

        public readonly Getters.MemberFormatKey<TStructure, TMember>    MemberFormatKey;
        public readonly   Getters.MemberDefaultValue<TStructure, TMember> MemberDefaultValue;

        // Unrequired delegates 

        public readonly Getters.MemberClasses<TStructure, TMember>?          MemberClasses;
        public readonly Getters.MemberIsEnabled<TStructure, TMember>?        MemberIsEnabled;
        public readonly Getters.MemberIsVisible<TStructure, TMember>?        MemberIsVisible;
        public readonly Getters.MemberInputIsRequired<TStructure, TMember>?  MemberInputIsRequired;
        public readonly Getters.MemberInputPlaceholder<TStructure, TMember>? MemberInputPlaceholder;
        public readonly Getters.MemberValidations<TStructure, TMember>?      MemberValidator;

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
    }
}