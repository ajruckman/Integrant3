using System;
using System.Collections.Generic;

namespace Fundament
{
    public class Structure<T>
    {
        public Structure(
            Getters.StructureClasses<T>?     structureClasses   = null,
            Getters.StructureIsVisible<T>?   structureIsVisible = null,
            Getters.StructureValidations<T>? structureValidator = null
        )
        {
            Members = new Dictionary<string, IMember<T>>();

            StructureClasses   = structureClasses;
            StructureIsVisible = structureIsVisible;
            StructureValidator = structureValidator;
        }

        internal readonly Getters.StructureClasses<T>?     StructureClasses;
        internal readonly Getters.StructureIsVisible<T>?   StructureIsVisible;
        internal readonly Getters.StructureValidations<T>? StructureValidator;

        public void Register<TMember>(Member<T, TMember> member)
        {
            if (member == null)
                throw new ArgumentNullException($"Cannot register null member.");

            if (Members.ContainsKey(member.ID))
                throw new ArgumentException($"Member with ID '{member.ID}' has already been registered.");

            Members[member.ID] = member;
        }

        private Dictionary<string, IMember<T>> Members { get; }

        public Member<T, TMember> Get<TMember>(string id)
        {
            Members.TryGetValue(id, out IMember<T>? member);
            if (member == null)
                throw new ArgumentException($"Member with ID '{id} has not been registered.");

            if (!(member is Member<T, TMember> result))
                throw new ArgumentException(
                    $"Member with ID '{id}' cannot be cast to a Member<TStructure, TMember> with the specified type arguments.");

            return result;
        }
    }
}