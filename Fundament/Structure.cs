using System;
using System.Collections.Generic;
using System.Linq;

namespace Fundament
{
    public class Structure<T>
    {
        public Structure
        (
            Getters.StructureClasses<T>?     classes   = null,
            Getters.StructureIsVisible<T>?   isVisible = null,
            Getters.StructureValidations<T>? validator = null
        )
        {
            Members = new Dictionary<string, IMember<T>>();

            StructureClasses   = classes;
            StructureIsVisible = isVisible;
            StructureValidator = validator;

            ValidationState = new ValidationState<T>(this);
        }

        internal readonly Getters.StructureClasses<T>?     StructureClasses;
        internal readonly Getters.StructureIsVisible<T>?   StructureIsVisible;
        internal readonly Getters.StructureValidations<T>? StructureValidator;

        internal readonly ValidationState<T> ValidationState;

        public void Register<TMember>(Member<T, TMember> member)
        {
            if (member == null)
                throw new ArgumentNullException($"Cannot register null member.");

            if (Members.ContainsKey(member.ID))
                throw new ArgumentException($"Member with ID '{member.ID}' has already been registered.");

            Members[member.ID] = member;
            member.OnValueUpdate += (value, member, memberValue) =>
                OnMemberValueUpdate?.Invoke(value, member, memberValue);
            member.OnValueUpdate += (value, member, memberValue) => ValidationState.ValidateStructure(value);
        }

        private Dictionary<string, IMember<T>> Members { get; }

        public Member<T, TMember> GetMember<TMember>(string id)
        {
            Members.TryGetValue(id, out IMember<T>? member);
            if (member == null)
                throw new ArgumentException($"Member with ID '{id} has not been registered.");

            if (!(member is Member<T, TMember> result))
                throw new ArgumentException(
                    $"Member with ID '{id}' cannot be cast to a Member<TStructure, TMember> with the specified type arguments.");

            return result;
        }

        public IEnumerable<IMember<T>> AllMembers() => Members.Values;

        public event Action<T, IMember<T>, object>? OnMemberValueUpdate;
    }
}