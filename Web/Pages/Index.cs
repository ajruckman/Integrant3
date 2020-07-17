using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fundament;
using Fundament.Input;

namespace Web.Pages
{
    public partial class Index
    {
        public class User
        {
            public bool     Boolean     { get; set; }
            public int      UserID      { get; set; }
            public string   Name        { get; set; }
            public string   PhoneNumber { get; set; }
            public string   Email       { get; set; }
            public DateTime StartDate   { get; set; }
        }

        private Structure<User> _structure = null!;
        private User            _testUser  = null!;

        protected override void OnInitialized()
        {
            var altUser = new User();

            _structure = new Structure<User>(validator: ((structure, value) =>
            {
                Thread.Sleep(500);
                return new List<Validation>
                {
                    new Validation(ValidationResultType.Warning, "asdf"),
                };
            }));

            _structure.Register(new Member<User, bool>
            (
                nameof(User.Boolean),
                (s,                v, m) => v.Boolean,
                onValueUpdate: (s, m, v) => s.Boolean = v
            ));

            _structure.Register(new Member<User, int>(
                nameof(User.UserID),
                (s,                  v, m) => $"[{v.UserID}]",
                memberFormatKey: (s, m, v) => "User ID",
                onValueUpdate: (s,   m, v) => s.UserID = v
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Name),
                (s, m, v) => m.Name,
                input: new StringInput<User>(),
                onValueUpdate: (s, m, v) => s.Name = v
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.PhoneNumber),
                (s,                  v, m) => v.PhoneNumber,
                memberFormatKey: (s, v, m) => "Phone number",
                memberIsVisible: (s, v, m) => v.Name?.Length > 0,
                onValueUpdate: (s,   m, v) => s.PhoneNumber = v
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Email),
                (s, m, v) => m.Email,
                input: new StringInput<User>(textArea: true, monospace: true),
                onValueUpdate: (s, m, v) => s.Email = v,
                memberValidator: (s, m, v) =>
                {
                    Thread.Sleep(250);
                    return string.IsNullOrEmpty(m.Email)
                        ? Validation.One(ValidationResultType.Warning, "Email is recommended.")
                        : m.Email.Contains("@")
                            ? Validation.One(ValidationResultType.Valid,   "Valid")
                            : Validation.One(ValidationResultType.Invalid, "Invalid");
                }));

            _structure.Register(new Member<User, DateTime>(
                nameof(User.StartDate),
                (s,                m, v) => m.StartDate,
                onValueUpdate: (s, m, v) => s.StartDate = v,
                memberValidator: (s, m, v) =>
                    m.StartDate > DateTime.Now
                        ? Validation.One(ValidationResultType.Invalid, "Start date is in the future.")
                        : Validation.One(ValidationResultType.Valid,   "Valid"),
                input: new DateInput<User>()
            ));

            //

            _structure.GetMember<string>("Name").OnValueUpdate +=
                (s, m, v) => Console.WriteLine($"Structure<User>." + m.ID + " -> " + v);

            _structure.GetMember<string>("Email").OnValueUpdate +=
                (s, m, v) => Console.WriteLine($"Structure<User>." + m.ID + " -> " + v);

            _structure.OnMemberValueUpdate += (s, m, v) =>
            {
                Console.WriteLine($"Structure<User>." + m.ID + " -> " + v);
                InvokeAsync(StateHasChanged);
            };

            //

            _testUser = new User
            {
                Boolean     = true,
                UserID      = 12345,
                Name        = "A.J.",
                PhoneNumber = "111.222.3344",
                Email       = "aj@example.com",
            };

            Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Updating");
                _testUser.Boolean = false;
                InvokeAsync(StateHasChanged);
            });
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) { }
        }
    }
}