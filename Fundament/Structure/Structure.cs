using System;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace Integrant.Fundament.Structure
{
    public sealed class Structure<T>
    {
        public readonly StructureGetters.StructureClasses<T>?     Classes;
        public readonly StructureGetters.StructureIsVisible<T>?   IsVisible;
        public readonly StructureGetters.StructureValidations<T>? Validator;

        private readonly Dictionary<string, IMember<T>> _members;

        public Structure
        (
            StructureGetters.StructureClasses<T>?     classes   = null,
            StructureGetters.StructureIsVisible<T>?   isVisible = null,
            StructureGetters.StructureValidations<T>? validator = null
        )
        {
            _members = new Dictionary<string, IMember<T>>();

            Classes   = classes;
            IsVisible = isVisible;
            Validator = validator;
        }

        public void Register<TMember>(Member<T, TMember> member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member), "Cannot register null member.");

            if (_members.ContainsKey(member.ID))
                throw new ArgumentException($"Member with ID '{member.ID}' has already been registered.");

            _members[member.ID] = member;
        }

        public StructureInstance<T> Instantiate(IJSRuntime? jsRuntime = null) => new StructureInstance<T>(this, jsRuntime);

        public Member<T, TMember> GetMember<TMember>(string id)
        {
            _members.TryGetValue(id, out IMember<T>? member);
            if (member == null)
                throw new ArgumentException($"Member with ID '{id} has not been registered.");

            if (!(member is Member<T, TMember> result))
                throw new ArgumentException(
                    $"Member with ID '{id}' cannot be cast to a Member<TStructure, TMember> with the specified type arguments.");

            return result;
        }

        //

        public IEnumerable<IMember<T>> AllMembers() => _members.Values;
    }
}