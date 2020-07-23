using System;

namespace Integrant.Fundament
{
    public class StructureState<T>
    {
        public readonly Structure<T>       Structure;
        public readonly ValidationState<T> ValidationState;

        internal StructureState(Structure<T> structure)
        {
            Structure       = structure;
            ValidationState = new ValidationState<T>(this);

            foreach (IMember<T> member in structure.AllMembers())
            {
                member.OnInput          += s => ValidationState.Invalidate();
                member.OnInputDebounced += s => ValidationState.ValidateStructure(s);
            }
        }

        //

        private readonly object _validatedInitialLock = new object();
        private          bool   _hasValidatedInitial;

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
        // //
        //
        // private readonly object _validatedInitialLock = new object();
        // private          bool   _hasValidatedInitial;
        //
        // public void ValidateInitial(T value)
        // {
        //     lock (_validatedInitialLock)
        //     {
        //         if (!_hasValidatedInitial)
        //         {
        //             ValidationState.ValidateStructure(value);
        //             _hasValidatedInitial = true;
        //         }
        //     }
        // }
        //
        // public void Revalidate(T value)
        // {
        //     ValidationState.Invalidate();
        //     ValidationState.ValidateStructure(value);
        // }

        //

        public event Action? OnResetAllMemberInputs;

        public void ResetAllMemberInputs(T value)
        {
            foreach (IMember<T> member in Structure.AllMembers())
            {
                member.ResetInputs(value);
            }

            OnResetAllMemberInputs?.Invoke();
        }
    }
}