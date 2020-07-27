using System;
using System.Collections.Generic;
using System.Linq;
using Integrant.Fundament.Structure;

namespace Integrant.Fundament
{
    public sealed class ClassSet
    {
        private List<string> _classes;
        private string?      _formatted;

        public ClassSet(params string[] classes)
        {
            _classes = classes.ToList();
        }

        private ClassSet(List<string> classes)
        {
            _classes = classes;
        }

        public void Add(string c)
        {
            if (_formatted != null) throw new Exception("ClassSet has already been finalized.");
            _classes.Add(c);
        }

        public void AddRange(IEnumerable<string> c)
        {
            if (_formatted != null) throw new Exception("ClassSet has already been finalized.");
            _classes.AddRange(c);
        }

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
                primary + "#" + typeof(TS).FullName + "@" + member.ID,
            };

            classes.AddRange(additional);

            if (member.Classes != null)
                classes.AddRange(member.Classes.Invoke(structure, value, member));

            return new ClassSet {_classes = classes};
        }

        public string Format()
        {
            if (_formatted == null)
            {
                _formatted = string.Join(' ', _classes);
            }

            return _formatted;
        }

        public ClassSet Clone()
        {
            return new ClassSet(_classes.ToList());
        }
    }
}