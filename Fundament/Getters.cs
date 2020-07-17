using System.Collections.Generic;

namespace Fundament
{
    public static class Getters
    {
        public delegate List<string> StructureClasses<T>
            (Structure<T> structure, T value);

        public delegate bool StructureIsVisible<T>
            (Structure<T> structure, T value);

        public delegate List<Validation> StructureValidations<T>
            (Structure<T> structure, T value);

        //

        public delegate object MemberFormatValue<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate object MemberFormatKey<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate object MemberDefaultValue<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        //

        public delegate List<string> MemberClasses<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberIsEnabled<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberIsVisible<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate bool MemberInputIsRequired<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate string MemberInputPlaceholder<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);

        public delegate List<Validation> MemberValidator<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member);
    }

    public static class DefaultGetters
    {
        public static string FormatMemberKey<TStructure, TMember>
            (Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member)
        {
            return member.ID;
        }
    }
}