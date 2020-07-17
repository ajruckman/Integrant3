using System;
using System.Collections.Generic;
using Fundament.Input;
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
            Getters.MemberValidator<TStructure, TMember>?             memberValidator        = null,
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

        internal readonly Getters.MemberFormatValue<TStructure, TMember> MemberFormatValue;

        // Required delegates with fallback default implementations

        internal readonly Getters.MemberFormatKey<TStructure, TMember>    MemberFormatKey;
        internal readonly Getters.MemberDefaultValue<TStructure, TMember> MemberDefaultValue;

        // Unrequired delegates 

        internal readonly Getters.MemberClasses<TStructure, TMember>?          MemberClasses;
        internal readonly Getters.MemberIsEnabled<TStructure, TMember>?        MemberIsEnabled;
        internal readonly Getters.MemberIsVisible<TStructure, TMember>?        MemberIsVisible;
        internal readonly Getters.MemberInputIsRequired<TStructure, TMember>?  MemberInputIsRequired;
        internal readonly Getters.MemberInputPlaceholder<TStructure, TMember>? MemberInputPlaceholder;
        internal readonly Getters.MemberValidator<TStructure, TMember>?        MemberValidator;

        //

        private readonly Debouncer<(TStructure, TMember)> _debouncer;

        public event Action<TStructure, Member<TStructure, TMember>, TMember>? OnValueUpdate;

        public void UpdateValue(TStructure value, TMember newValue)
        {
            _debouncer.Reset((value, newValue));
        }

        //

        public List<Validation>? Validations(Structure<TStructure> structure, TStructure value)
        {
            return MemberValidator?.Invoke(structure, value, this);
        }
    }
}