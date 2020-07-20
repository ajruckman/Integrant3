using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fundament;
using Rudiment.Input;

namespace Web.Pages
{
    public partial class Index
    {
        public class User
        {
            public bool     Boolean     { get; set; }
            public string   CreatedBy   { get; set; }
            public int      UserID      { get; set; }
            public string   Name        { get; set; }
            public string   PhoneNumber { get; set; }
            public string   Email       { get; set; }
            public DateTime StartDate   { get; set; }
            public DateTime StartTime   { get; set; }
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
                    new Validation(ValidationResultType.Warning, "Overall validation"),
                };
            }));

            _structure.Register(new Member<User, bool>
            (
                nameof(User.Boolean),
                (s,                v, m) => v.Boolean,
                onValueUpdate: (s, v, m) => s.Boolean = m
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.CreatedBy),
                (s,                  v, m) => v.CreatedBy,
                onValueUpdate: (s,   v, m) => s.CreatedBy = m,
                memberIsVisible: (s, v, m) => v.Boolean,
                input: new StringInput<User>()
            ));

            _structure.Register(new Member<User, int>(
                nameof(User.UserID),
                (s,                  v, m) => $"[{v.UserID}]",
                memberFormatKey: (s, v, m) => "User ID",
                onValueUpdate: (s,   v, m) => s.UserID = m
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Name),
                (s, v, m) => v.Name,
                input: new StringInput<User>(),
                onValueUpdate: (s, v, m) => s.Name = m
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.PhoneNumber),
                (s,                  v, m) => v.PhoneNumber,
                memberFormatKey: (s, v, m) => "Phone number",
                memberIsVisible: (s, v, m) => v.Name?.Length > 0,
                onValueUpdate: (s,   v, m) => s.PhoneNumber = m
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Email),
                (s, v, m) => v.Email,
                input: new StringInput<User>(textArea: true, monospace: true),
                onValueUpdate: (s, v, m) => s.Email = m,
                memberValidator: (s, v, m) =>
                {
                    Thread.Sleep(250);
                    return string.IsNullOrEmpty(v.Email)
                        ? Validation.One(ValidationResultType.Warning, "Email is recommended.")
                        : v.Email.Contains("@")
                            ? Validation.One(ValidationResultType.Valid,   "Valid")
                            : Validation.One(ValidationResultType.Invalid, "Invalid");
                }));

            _structure.Register(new Member<User, DateTime>(
                nameof(User.StartDate),
                (s,                v, m) => v.StartDate,
                onValueUpdate: (s, v, m) => s.StartDate = m,
                memberValidator: (s, v, m) =>
                    v.StartDate > DateTime.Now
                        ? Validation.One(ValidationResultType.Invalid, "Start date is in the future.")
                        : Validation.One(ValidationResultType.Valid,   "Valid"),
                input: new DateInput<User>()
            ));

            _structure.Register(new Member<User, DateTime>(
                nameof(User.StartTime),
                (s,                v, m) => v.StartTime,
                onValueUpdate: (s, v, m) => s.StartTime = m,
                input: new TimeInput<User>()
            ));

            //

            _structure.GetMember<string>("Name").OnValueUpdate +=
                (s, v, m) => Console.WriteLine($"Structure<User>." + v.ID + " -> " + v);

            _structure.GetMember<string>("Email").OnValueUpdate +=
                (s, v, m) => Console.WriteLine($"Structure<User>." + v.ID + " -> " + v);

            _structure.OnMemberValueUpdate += (s, v, m) =>
            {
                Console.WriteLine($"Structure<User>." + v.ID + " -> " + v);
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