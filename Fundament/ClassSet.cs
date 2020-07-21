using System.Collections.Generic;

namespace Integrant.Fundament
{
    public class ClassSet
    {
        private List<string> _classes;

        public static ClassSet FromStructure<TS>
            (Structure<TS> structure, TS value, string primary, params string[] additional)
        {
            var classes = new List<string>(additional.Length + 1)
            {
                primary,
                primary + "#" + typeof(TS).FullName,
            };

            classes.AddRange(additional);

            if (structure.StructureClasses != null)
                classes.AddRange(structure.StructureClasses.Invoke(structure, value));

            return new ClassSet {_classes = classes};
        }

        public static ClassSet FromMember<TS, TM>
            (Structure<TS> structure, TS value, Member<TS, TM> member, string primary, params string[] additional)
        {
            var classes = new List<string>(additional.Length + 1)
            {
                primary,
                primary + "@" + member.ID,
            };

            classes.AddRange(additional);

            if (member.MemberClasses != null)
                classes.AddRange(member.MemberClasses.Invoke(structure, value, member));

            return new ClassSet {_classes = classes};
        }

        public override string ToString()
        {
            return string.Join(' ', _classes);
        }
    }
}