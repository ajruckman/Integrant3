using System;
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
            public bool   Boolean     { get; set; }
            public int    UserID      { get; set; }
            public string Name        { get; set; }
            public string PhoneNumber { get; set; }
            public string Email       { get; set; }
        }

        private Structure<User> _structure = null!;
        private User            _testUser  = null!;

        protected override void OnInitialized()
        {
            _structure = new Structure<User>();

            _structure.Register(new Member<User, bool>
            (
                nameof(User.Boolean),
                (s, v, m) => v.Boolean
            ));

            _structure.Register(new Member<User, int>(
                nameof(User.UserID),
                (s,                  v, m) => $"[{v.UserID}]",
                memberFormatKey: (s, v, m) => "User ID"
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.Name),
                (s, v, m) => v.Name,
                input: new StringInput<User>()
            ));

            _structure.Register(new Member<User, string>(
                nameof(User.PhoneNumber),
                (s,                  v, m) => v.PhoneNumber,
                memberFormatKey: (s, v, m) => "Phone number",
                memberIsVisible: (s, v, m) => v.Name?.Length > 0)
            );

            _structure.Register(new Member<User, string>(
                nameof(User.Email),
                (s, v, m) => v.Email,
                input: new StringInput<User>(textArea: true, monospace: true)
            ));

            // var m = new Member<User, string>(nameof(User.Name));

            _structure.Get<string>("Name").OnValueUpdate += s =>
            {
                Console.WriteLine($"Structure<User>.Name  -> " + s);
            };
            _structure.Get<string>("Email").OnValueUpdate += s =>
            {
                Console.WriteLine($"Structure<User>.Email -> " + s);
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