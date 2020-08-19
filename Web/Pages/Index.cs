using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlareSelect;
using Integrant.Element.Bits;
using Integrant.Element.Constructs;
using Integrant.Fundament.Element;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;
using Superset.Common;

namespace Integrant.Web.Pages
{
    public partial class Index
    {
        [CascadingParameter(Name = "PrimaryHeader")]
        public Header PrimaryHeader { get; set; } = null!;

        private StructureInstance<User> _structure    = null!;
        private FlareSelector<string>   _tagsSelector = null!;
        private User                    _testUser     = null!;

        private const bool DoSlow = false;

        private Header _header2 = null!;

        private Button       _submitButton             = null!;

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
                StartDate    = DateTime.Now,
                StartTime    = DateTime.Now,
                DepartmentID = 2,
                DepartmentType2 = 2,
            };
            //

            _structure = Common.Structure.Instantiate(JSRuntime);

            //

            _structure.GetMemberInstance<string>("Name").OnValueUpdateUntyped +=
                (v, m, mv) => Console.WriteLine($"Structure<User>." + m.ID + " -> " + v);

            _structure.GetMemberInstance<string>("Email").OnValueUpdateUntyped +=
                (v, m, mv) => Console.WriteLine($"Structure<User>." + m.ID + " -> " + v);

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

            //

            _submitButton = new Button(() => "Submit", _ => Submit(), () => Button.Color.Green,
                isDisabled: () => !_structure.ValidationState.Valid());

            _header2 = new Header
            (
                new List<IBit>
                {
                    new Title(() => "Header #1!"),
                    new Filler(),
                    new Link(() => "Link 1!", () => "/url1"),
                    new Space(),
                    new Link(() => "Link 2!", () => "/url2", isHighlighted: () => true),
                    new Space(),
                    new Link(() => "Link 3!", () => "/url3"),
                },
                Header.HeaderType.Secondary
            );

            // Task.Run(async () =>
            // {
            //     while (true)
            //     {
            //         await InvokeAsync(StateHasChanged);
            //         Thread.Sleep(200);
            //     }
            // });
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine("---");
            // throw new Exception();
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