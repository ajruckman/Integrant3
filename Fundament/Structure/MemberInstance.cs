using System;
using System.Collections.Generic;

namespace Integrant.Fundament.Structure
{
    public interface IMemberInstance<TStructure>
    {
        public string ID { get; }

        public event Action?                                          OnInput;
        public event Action?                                          OnResetInputs;
        public event Action<TStructure, IMember<TStructure>, object>? OnValueUpdate;

        public void              ResetInputs();
        public List<Validation>? Validations(Structure<TStructure> structure, TStructure value);
    }

    public sealed class MemberInstance<TStructure, TMember> : IMemberInstance<TStructure>
    {
        private readonly Utility.Debouncer<(TStructure, TMember)> _debouncer;

        internal MemberInstance(Member<TStructure, TMember> member)
        {
            Member = member;

            if (Member.Input != null)
            {
                Input         =  Member.Input.Invoke();
                Input.OnInput += UpdateValue;
            }

            if (Member.OnValueUpdate != null)
                OnValueUpdate += (s, v, m) => Member.OnValueUpdate?.Invoke(s, v, (TMember) m);

            _debouncer = new Utility.Debouncer<(TStructure, TMember)>(newValue =>
                    OnValueUpdate?.Invoke(newValue.Item1, Member, newValue.Item2), default!,
                Member.InputDebounceMilliseconds);
        }

        public Member<TStructure, TMember> Member { get; }

        public IInput<TStructure, TMember>? Input { get; }

        //

        public string ID => Member.ID;

        public event Action? OnInput;

        //

        public List<Validation>? Validations
            (Structure<TStructure> structure, TStructure value) => Member.Validator?.Invoke(structure, value, Member);

        //

        public event Action? OnResetInputs;

        public void ResetInputs()
        {
            OnResetInputs?.Invoke();
        }

        public event Action? OnRerenderInputs;

        public void RerenderInputs()
        {
            OnRerenderInputs?.Invoke();
        }

        //

        public event Action<TStructure, IMember<TStructure>, object>? OnValueUpdate;

        public void UpdateValue(TStructure value, TMember newValue)
        {
            OnInput?.Invoke();
            _debouncer.Reset((value, newValue));
        }

        public void UpdateValueImmediately(TStructure value, TMember newValue)
        {
            OnValueUpdate?.Invoke(value, Member, newValue);
        }
    }
}