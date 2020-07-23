using System.Collections.Generic;
using System.Linq;

namespace Integrant.Fundament
{
    public sealed class ClassSet
    {
        private List<string> _classes;

        public ClassSet(params string[] classes)
        {
            _classes = classes.ToList();
        }

        public void Add(string c) => _classes.Add(c);

        public static ClassSet FromStructure<TS>
            (Structure<TS> structure, TS value, string primary, params string[] additional)
        {
            var classes = new List<string>(additional.Length + 1)
            {
                primary,
                primary + "#" + typeof(TS).FullName,
            };

            classes.AddRange(additional);

            if (structure.Classes != null)
                classes.AddRange(structure.Classes.Invoke(structure, value));

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

            if (member.Classes != null)
                classes.AddRange(member.Classes.Invoke(structure, value, member));

            return new ClassSet {_classes = classes};
        }

        public string Format()
        {
            return string.Join(' ', _classes);
        }
    }
}