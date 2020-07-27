using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlareSelect;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Integrant.Rudiment.Inputs;
using Superset.Common;

namespace Integrant.Web.Pages
{
    public partial class Index
    {
        public class User
        {
            public bool         Boolean           { get; set; }
            public string       CreatedBy         { get; set; }
            public int          UserID            { get; set; }
            public string       Name              { get; set; }
            public string       PhoneNumber       { get; set; }
            public string       Email             { get; set; }
            public DateTime?    StartDate         { get; set; }
            public DateTime     StartTime         { get; set; }
            public DateTime?    CompositeDateTime { get; set; }
            public List<string> Tags              { get; set; }
        }

        private StructureInstance<User> _structure    = null!;
        private FlareSelector<string>   _tagsSelector = null!;
        private User                    _testUser     = null!;

        private const bool DoSlow = false;

        protected override void OnInitialized()
        {
            var altUser = new User();
            //

            _testUser = new User
            {
                Boolean     = true,
                UserID      = 12345,
                Name        = "A.J.",
                PhoneNumber = "111.222.3344",
                Email =
                    "aj@example.com9999999999999999999999999999999999!99999999999999999999999999999999999999999999999999999999999999999! spaced out words n stuff",
                StartDate = DateTime.Now,
                StartTime = DateTime.Now,
            };
            //

            _structure = Common.Structure.Instantiate();

            //

            _structure.GetMemberInstance<string>("Name").OnValueUpdate +=
                (s, v, m) => Console.WriteLine($"Structure<User>." + v.ID + " -> " + v);

            _structure.GetMemberInstance<string>("Email").OnValueUpdate +=
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
                _structure.GetMemberInstance<List<string>>(nameof(User.Tags))
                          .UpdateValue(_testUser, selected.Select(v => v.ID).ToList());

            _structure.OnResetAllMemberInputs += () => _tagsSelector.Deselect();

            _structure.ValidationState.OnFinishValidating += () => InvokeAsync(StateHasChanged);



            Task.Run(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Updating");
                _testUser.Boolean = false;
                _structure.GetMemberInstance<bool>(nameof(User.Boolean)).ResetInputs();
                _structure.Revalidate(_testUser);
                InvokeAsync(StateHasChanged);
            });
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) { }
        }

        private void ResetAll()
        {
            _structure.ResetAllMemberInputs();
        }

        private void Submit()
        {
            Console.WriteLine("Submit");
        }
    }
}