using System;
using Integrant.Fundaments;

namespace TestProgram
{
    internal static class Program
    {
        public class User
        {
            public bool   Boolean     { get; set; }
            public int    UserID      { get; set; }
            public string Name        { get; set; }
            public string PhoneNumber { get; set; }
            public string Email       { get; set; }
        }

        private static void Main()
        {
            var structure = new Structure<User>();

            var testUser = new User
            {
                Boolean     = true,
                UserID      = 12345,
                Name        = "A.J.",
                PhoneNumber = "111.222.3344",
                Email       = "aj@example.com",
            };

            // structure.Register(new Member<User, bool>(nameof(User.Boolean)));
            // structure.Register(new Member<User, int>(nameof(User.UserID)));
            // structure.Register(new Member<User, string>(nameof(User.Name)));
            // structure.Register(new Member<User, string>(nameof(User.PhoneNumber),
            //                                             memberIsVisible: (s, v, m) => v.Name?.Length > 0));
            // structure.Register(new Member<User, string>(nameof(User.Email)));

            // structure.MemberSet = MemberSet.FromClass<User>(testUser);

            // foreach (IMember<User> memberSetMember in structure.MemberSet.Members)
            // {
            //     Console.WriteLine(memberSetMember.ID);
            // }
        }
    }
}