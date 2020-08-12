using System;
using System.Collections.Generic;

namespace Integrant.Fundament.Structure
{
    public interface IMemberInstance<TStructure>
    {
        public string ID { get; }

        public List<Validation>? Validations(TStructure value);

        // Events

        public event Action?                                           OnInput;
        public event Action<TStructure, IMember<TStructure>, object?>? OnValueUpdateUntyped;

        public void          RefreshInputs();
        public event Action? OnRefreshInputs;
        public void          ResetInputs();
        public event Action? OnResetInputs;
    }

    // public interface IMemberInstance<TStructure, TMember> : IMemberInstance<TStructure>
    // {
    //     public Member<TStructure, TMember>  Member { get; }
    //     public IInput<TStructure, TMember>? Input  { get; }
    //
    //     public event Action<TStructure, Member<TStructure, TMember>, TMember>? OnValueUpdate;
    //
    //     public void UpdateValue(TStructure            value, TMember newValue);
    //     public void UpdateValueImmediately(TStructure value, TMember newValue);
    // }

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

            OnValueUpdate += (v, m, mv) => OnValueUpdateUntyped?.Invoke(v, m, mv);

            if (Member.OnValueUpdate != null)
                OnValueUpdate += (v, m, mv) => Member.OnValueUpdate?.Invoke(v, m, mv);

            _debouncer = new Utility.Debouncer<(TStructure, TMember)>(newValue =>
                    OnValueUpdate?.Invoke(newValue.Item1, Member, newValue.Item2),
                default!,
                Member.InputDebounceMilliseconds
            );
        }

        public string                       ID     => Member.ID;
        public Member<TStructure, TMember>  Member { get; }
        public IInput<TStructure, TMember>? Input  { get; }

        //

        public List<Validation>? Validations(TStructure value) => Member.Validator?.Invoke(value, Member);

        public event Action? OnInput;

        public event Action<TStructure, IMember<TStructure>, object?>?         OnValueUpdateUntyped;
        public event Action<TStructure, Member<TStructure, TMember>, TMember>? OnValueUpdate;

        public void RefreshInputs()
        {
            OnRefreshInputs?.Invoke();
        }

        public event Action? OnRefreshInputs;

        public void ResetInputs()
        {
            OnResetInputs?.Invoke();
        }

        public event Action? OnResetInputs;

        //

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