using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlareSelect;
using Integrant.Fundament;
using Integrant.Rudiment.Input;
using Superset.Common;

namespace Integrant.Web.Pages
{
    public partial class Index
    {
        public class User
        {
            public bool         Boolean     { get; set; }
            public string       CreatedBy   { get; set; }
            public int          UserID      { get; set; }
            public string       Name        { get; set; }
            public string       PhoneNumber { get; set; }
            public string       Email       { get; set; }
            public DateTime     StartDate   { get; set; }
            public DateTime     StartTime   { get; set; }
            public List<string> Tags        { get; set; }
        }

        private Structure<User>       _structure    = null!;
        private FlareSelector<string> _tagsSelector = null!;
        private User                  _testUser     = null!;

        public bool BindTestProp { get; set; }

        protected override void OnInitialized()
        {
            var altUser = new User();
            
            _structure = new Structure<User>(validator: (structure, value) =>
            {
                Thread.Sleep(200);
                return new List<Validation>
                {
                    new Validation(ValidationResultType.Warning, "Overall validation"),
                };
            });

            _structure.Register(new Member<User, bool>
            (
                nameof(User.Boolean),
                (s,                v, m) => v.Boolean,
                onValueUpdate: (s, v, m) => altUser.Boolean = m,
                input: new CheckboxInput<User>()
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.CreatedBy),
                (s,                v, m) => v.CreatedBy,
                onValueUpdate: (s, v, m) => altUser.CreatedBy = m,
                isVisible: (s,     v, m) => altUser.Boolean,
                input: new StringInput<User>()
            ));

            _structure.Register(new Member<User, int>(
                nameof(User.UserID),
                (s,                v, m) => v.UserID,
                formatValue: (s,   v, m) => $"[{v.UserID}]",
                formatKey: (s,     v, m) => "User ID",
                onValueUpdate: (s, v, m) => altUser.UserID = m
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Name),
                (s, v, m) => v.Name,
                input: new StringInput<User>(),
                onValueUpdate: (s, v, m) => altUser.Name = m,
                defaultValue: (s,  v, m) => "A.J."
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.PhoneNumber),
                (s,                v, m) => v.PhoneNumber,
                formatKey: (s,     v, m) => "Phone number",
                isVisible: (s,     v, m) => v.Name?.Length > 0,
                onValueUpdate: (s, v, m) => altUser.PhoneNumber = m
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Email),
                (s, v, m) => v.Email,
                input: new StringInput<User>(textArea: true, monospace: true),
                onValueUpdate: (s, v, m) => altUser.Email = m,
                validator: (s, v, m) =>
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
                onValueUpdate: (s, v, m) => altUser.StartDate = m,
                validator: (s, v, m) =>
                    v.StartDate > DateTime.Now
                        ? Validation.One(ValidationResultType.Invalid, "Start date is in the future.")
                        : Validation.One(ValidationResultType.Valid,   "Valid"),
                input: new DateInput<User>()
            ));

            _structure.Register(new Member<User, DateTime>(
                nameof(User.StartTime),
                (s,                v, m) => v.StartTime,
                onValueUpdate: (s, v, m) => altUser.StartTime = m,
                input: new TimeInput<User>()
            ));

            _structure.Register(new Member<User, List<string>>(
                nameof(User.Tags),
                (s,                v, m) => v.Tags,
                formatValue: (s,   v, m) => v.Tags != null ? string.Join(" + ", v.Tags) : "<null>",
                onValueUpdate: (s, v, m) => altUser.Tags = m
            ));

            //

            _structure.GetMember<string>("Name").OnValueUpdate +=
                (s, v, m) => Console.WriteLine($"Structure<User>." + v.ID + " -> " + v);

            _structure.GetMember<string>("Email").OnValueUpdate +=
                (s, v, m) => Console.WriteLine($"Structure<User>." + v.ID + " -> " + v);

            _structure.OnMemberValueUpdate += (s, v, m) =>
            {
                Console.WriteLine($"Structure<User>." + v.ID + " -> " + m);
                InvokeAsync(StateHasChanged);
            };

            //

            // _structure.OnMemberValueUpdate += (s, m, v) => InvokeAsync(StateHasChanged);

            _tagsSelector = new FlareSelector<string>
            (() => new List<IOption<string>>
                {
                    new Option<string> {ID = "A", OptionText = "A",},
                    new Option<string> {ID = "B", OptionText = "B",},
                    new Option<string> {ID = "C", OptionText = "C",},
                },
                true
            );

            _tagsSelector.OnSelect += selected =>
                _structure.GetMember<List<string>>(nameof(User.Tags))
                          .UpdateValue(altUser, selected.Select(v => v.ID).ToList());

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
                Thread.Sleep(500);
                Console.WriteLine("Updating");
                _testUser.Boolean = false;
                _structure.GetMember<bool>(nameof(User.Boolean)).ResetInputs();
                _structure.Revalidate(_testUser);
                InvokeAsync(StateHasChanged);

                Task.Run(() =>
                {
                    Thread.Sleep(500);
                    BindTestProp = true;
                });
            });
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) { }
        }

        private void ResetAll()
        {
            foreach (IMember<User> member in _structure.AllMembers())
            {
                member.ResetInputs();
            }

            StateHasChanged();
        }
    }
}