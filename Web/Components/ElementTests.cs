using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Bogus;
using Integrant.Colorant.Themes.Solids;
using Integrant.Element;
using Integrant.Element.Bits;
using Integrant.Element.Components.Combobox;
using Integrant.Element.Components.Modal;
using Integrant.Element.Components.Multibox;
using Integrant.Element.Constructs;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Integrant.Resources.Icons;
using Integrant.Web.Pages;
using Microsoft.AspNetCore.Components;

namespace Integrant.Web.Components
{
    public partial class ElementTests
    {
        [CascadingParameter]
        public Layer Layer { get; set; } = null!;

        private Layer _childLayer = null!;

        public double Number { get; set; }

        private Button       _colorChangingButton      = null!;
        private Button.Color _colorChangingButtonColor = Button.Color.Default;
        private ButtonGroup  _buttonGroup;
        private ButtonGroup  _buttonGroup2;

        private Modal _modal1 = null!;

        private Multibox<PopperTests.User> _multibox1 = null!;

        private HeaderGroup _headerGroup = null!;

        protected override void OnInitialized()
        {
            _childLayer = new Layer(Layer);

            _dropdown1 = new Dropdown
            (
                new Title(() => "Top titleZZ 1!"),
                new List<IBit>
                {
                    new Link(() => "Link 1!",                      () => "/"),
                    new Link(() => "Link 2!",                      () => "/"),
                    new Link(() => "Link longer than the others!", () => "/"),
                    new Arrow(
                        isStatic: false,
                        margin: () => new Size(5),
                        foregroundColor: () => Constants.Yellow_7,
                        fontSize: () => Number
                    ),
                    new Heading(
                        () => "H3 heading #1!", Heading.Size.H3, display: () => Display.InlineBlock,
                        margin: () => new Size(5, 10, 15), padding: () => new Size(3, 6),
                        fontWeight: () => 700, backgroundColor: () => Constants.Blue_7,
                        foregroundColor: () => Constants.Blue_7_Text
                    ),
                    new Title(
                        () => "Title #1!",
                        margin: () => new Size(5, 10, 15), padding: () => new Size(3, 6),
                        foregroundColor: () => Constants.Blue_7
                    ),
                    new Chip(
                        () => "A chip", backgroundColor: () => Constants.Green_4,
                        foregroundColor: () => Constants.Green_4_Text
                    ),
                    new Dropdown(
                        new Title(() => "Subtitle 1!"),
                        new List<IBit>
                        {
                            new Link(() => "Link 1!",                      () => "/"),
                            new Link(() => "Link 2!",                      () => "/"),
                            new Link(() => "Link longer than the others!", () => "/"),
                        },
                        true),
                }
            );

            Button.Color NextColor(Button.Color current)
            {
                return current == Button.Color.Yellow ? Button.Color.Default : current + 1;
            }

            _colorChangingButton = new Button
            (
                () => "Color changing button",
                _ =>
                {
                    _colorChangingButtonColor = NextColor(_colorChangingButtonColor);
                    StateHasChanged();
                },
                () => _colorChangingButtonColor,
                isStatic: false,
                data: () => new Dictionary<string, BitGetters.DataValue>
                {
                    {"next-color", () => NextColor(_colorChangingButtonColor).ToString()},
                }
            );

            _buttonGroup = new ButtonGroup(new List<Button>
            {
                new Button(() =>
                {
                    var arrowLeft = new Icon<MaterialIcon>("keyboard_arrow_right");
                    return "Button with content " + arrowLeft.Render().Content();
                }, _ => _modal1.Show(), pixelsHeight: () => 21.5, padding: () => new Size(2, 0, 2, 6)),
                new Button(() => "Button blue!", async _ => await Console.Out.WriteLineAsync("async"),
                    () => Button.Color.Blue),
                new Button(() => "Button green!",  _ => { }, () => Button.Color.Green, isDisabled: () => true),
                new Button(() => "Button orange!", _ => { }, () => Button.Color.Orange),
                new Button(() => "Button purple!", _ => { }, () => Button.Color.Purple, isHighlighted: () => true),
                new Button(() => "Button red!", _ => { }, () => Button.Color.Default,
                    tooltip: () => RandomNumberGenerator.GetInt32(100).ToString()),
                new Button(() => "Button yellow!", _ => { }, () => Button.Color.Default),
            });

            // _buttonGroup2 = new ButtonGroup(new List<Button>
            // {
            // new ToggleButton(() => "Toggle #1", async b => await Task.CompletedTask, () => false),
            // });

            _modal1 = new Modal();

            _faker = new Faker<PopperTests.User>()
                    .RuleFor(u => u.ID,         (f, u) => f.IndexFaker)
                    .RuleFor(u => u.Name,       (f, u) => f.Name.FullName())
                    .RuleFor(u => u.Department, (f, u) => f.Commerce.Department());

            _users = _faker.Generate(1000);
            _options = _users.Select(v => new PopperTests.Option
            {
                Key           = v.ID.ToString(),
                Value         = v,
                OptionText    = $"{v.Name} - {v.Department}",
                SelectionText = v.Name,
                // Selected      = v.Name.StartsWith("A"),
            }).ToList();

            _multibox1 = new Multibox<PopperTests.User>
            (
                JSRuntime, () => _options
            );

            var checkbox1Value = false;

            checkbox1 = new Checkbox(c => checkbox1Value = c, isChecked: () => checkbox1Value,
                tooltip: () => checkbox1Value ? "Checkbox is checked" : null);
            checkbox2 = new Checkbox(async c => await Console.Out.WriteLineAsync("async " + c), isChecked: () => true);
            checkbox3 = new Checkbox(c => { }, isDisabled: () => true);
            checkbox4 = new Checkbox(c => { }, isDisabled: () => true, isChecked: () => true);
            checkbox5 = new Checkbox(c => { }, isRequired: () => true);

            _headerGroup = new HeaderGroup(new List<Header>
            {
                new Header(new List<IBit>
                {
                    new Title(() => "Title of primary header"),
                    new Filler(),
                    new Chip(() => "Chip 1"),
                }),
                new Header(new List<IBit>
                {
                    new Title(() => "Title of primary header 2"),
                    new Filler(),
                    new Chip(() => "Chip 2"),
                }),
                new Header(new List<IBit>
                {
                    new TextLine(() => "TextLine of secondary header"),
                    new Filler(),
                    new Chip(() => "Chip 3"),
                }, Header.HeaderType.Secondary),
                new Header(new List<IBit>
                {
                    new TextLine(() => "TextLine of secondary header 2"),
                    new Filler(),
                    new Chip(() => "Chip 4"),
                }, Header.HeaderType.Secondary),
            }, () => 500);

            _combobox = new Combobox<PopperTests.User>(JSRuntime, () => _options);

            _combobox.OnSelect += selected =>
            {
                if (selected != null) { }
            };
        }

        Checkbox checkbox1 = null!;
        Checkbox checkbox2 = null!;
        Checkbox checkbox3 = null!;
        Checkbox checkbox4 = null!;
        Checkbox checkbox5 = null!;

        List<PopperTests.User> _users;

        List<PopperTests.Option> _options;

        private Combobox<PopperTests.User> _combobox = null!;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                // _multibox1.Select(_users[0]);
                // _multibox1._combobox.Select(_options[3]);
                // _multibox1._combobox.Select(_options[5]);
                // _multibox1._combobox.Select(_options[12]);
                // _multibox1._combobox.Select(_options[16]);
                // _multibox1._combobox.Select(_options[17]);
                // _modal1.Show();
            }
        }

        //

        private Dropdown _dropdown1 = null!;

        private Faker<PopperTests.User> _faker = null!;
    }
}