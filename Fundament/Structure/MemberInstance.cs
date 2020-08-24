using System;
using System.Collections.Generic;

namespace Integrant.Fundament.Structure
{
    public interface IMemberInstance<TStructure>
    {
        public string ID { get; }

        public List<Validation>? Validations(TStructure value);

        // Events

        /// <summary>
        /// Non-debounced event called as soon as new input is received.
        /// </summary>
        public event Action? OnInput;

        /// <summary>
        /// Untyped event that an IStructureInstance can subscribe to without knowing TMember.
        /// </summary>
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
                Input.OnInput += (v, m) => UpdateValue(v, m);
            }

            OnValueUpdate += (v, m, mv) =>
            {
                Member.ValueUpdater?.Invoke(v, m, mv);

                OnValueUpdateUntyped?.Invoke(v, m, mv);

                Member.OnValueUpdate?.Invoke(v, m, mv);
            };

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

        public void UpdateValue(TStructure value, TMember newValue, bool doTransform = true)
        {
            // if (Member.InputTransformer != null && doTransform)
                // newValue = Member.InputTransformer.Invoke(value, Member, newValue);

            OnInput?.Invoke();
            _debouncer.Reset((value, newValue));
        }

        public void UpdateValueImmediately(TStructure value, TMember newValue)
        {
            OnValueUpdate?.Invoke(value, Member, newValue);
        }
    }
}