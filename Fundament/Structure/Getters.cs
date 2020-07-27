using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace Integrant.Fundament.Structure
{
    public static class StructureGetters
    {
        public delegate List<string> StructureClasses<T>
            (Structure<T> structure, T value);

        public delegate bool StructureIsVisible<T>
            (Structure<T> structure, T value);

        public delegate List<Validation> StructureValidations<T>
            (Structure<T> structure, T value);
    }

    public static class MemberGetters
    {
        public delegate string MemberKey<TStructure, TMember>
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        );

        public delegate TMember MemberValue<TStructure, TMember>
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        );

        public delegate object MemberFormattedValue<TStructure, TMember>
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        );

        //

        public delegate IInput<TStructure, TMember> MemberInput<TStructure, TMember>();

        public delegate List<string> MemberClasses<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberIsVisible<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberInputIsDisabled<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberInputIsRequired<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberInputMeetsRequirement<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate string MemberInputPlaceholder<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate List<Validation> MemberValidations<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);
    }

    internal static class DefaultGetters
    {
        internal static string MemberKey<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member)
        {
            return member.ID;
        }

        internal static object MemberFormattedValue<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member)
        {
            TMember v = member.Value.Invoke(structure, value, member);
            return v == null ? (object) "" : v;
        }

        internal static bool MemberInputMeetsRequirement<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member)
        {
            TMember v = member.Value.Invoke(structure, value, member);

            if (v == null) return false;

            return typeof(TMember) == typeof(string)
                ? !Equals(v, "")
                : !Equals(v, default(TMember));
        }
    }
}