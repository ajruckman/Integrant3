using System;
using System.Collections.Generic;

namespace Integrant.Fundament.Structure
{
    public sealed class StructureInstance<T>
    {
        public readonly ValidationState<T> ValidationState;

        private readonly Dictionary<string, IMemberInstance<T>> _memberInstances;
        private readonly object                                 _validatedInitialLock = new object();
        private          bool                                   _hasValidatedInitial;

        internal StructureInstance(Structure<T> structure)
        {
            Structure        = structure;
            ValidationState  = new ValidationState<T>(this);
            _memberInstances = new Dictionary<string, IMemberInstance<T>>();

            foreach (IMember<T> member in structure.AllMembers())
            {
                IMemberInstance<T> instance = member.Instantiate();

                instance.OnInput       += ValidationState.Invalidate;
                instance.OnValueUpdate += (s, m, v) => OnMemberValueUpdate?.Invoke(s, m, v);
                instance.OnValueUpdate += (s, m, v) => ValidationState.ValidateStructure(s);

                _memberInstances[member.ID] = instance;
            }
        }

        public Structure<T> Structure { get; }

        public MemberInstance<T, TMember> GetMemberInstance<TMember>(string id)
        {
            _memberInstances.TryGetValue(id, out IMemberInstance<T>? member);
            if (member == null)
                throw new ArgumentException($"MemberInstance with ID '{id} has not been registered.");

            if (!(member is MemberInstance<T, TMember> result))
                throw new ArgumentException(
                    $"MemberInstance with ID '{id}' cannot be cast to a MemberInstance<TStructure, TMember> with the specified type arguments.");

            return result;
        }

        public void ValidateInitial(T value)
        {
            lock (_validatedInitialLock)
            {
                if (!_hasValidatedInitial)
                {
                    ValidationState.ValidateStructure(value);
                    _hasValidatedInitial = true;
                }
            }
        }

        public void Revalidate(T value)
        {
            ValidationState.Invalidate();
            ValidationState.ValidateStructure(value);
        }

        //

        public event Action<T, IMember<T>, object>? OnMemberValueUpdate;

        //

        public event Action? OnResetAllMemberInputs;

        public void ResetAllMemberInputs()
        {
            foreach (IMemberInstance<T> memberInstance in _memberInstances.Values)
            {
                memberInstance.ResetInputs();
            }

            OnResetAllMemberInputs?.Invoke();
        }

        //

        public IEnumerable<IMemberInstance<T>> AllMemberInstances() => _memberInstances.Values;
    }
}