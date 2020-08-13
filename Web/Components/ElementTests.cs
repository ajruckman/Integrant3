using System.Collections.Generic;
using Integrant.Colorant.Themes.Solids;
using Integrant.Element;
using Integrant.Element.Bits;
using Integrant.Element.Constructs;
using Integrant.Element.Layouts;
using Integrant.Fundament;
using Integrant.Fundament.Element;
using Integrant.Rudiment.Inputs;
using Microsoft.AspNetCore.Components;

namespace Integrant.Web.Components
{
    public partial class ElementTests
    {
        [CascadingParameter]
        public Layer Layer { get; set; } = null!;

        private Layer _childLayer = null!;

        public double Number { get; set; }

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
        }

        //

        private Dropdown _dropdown1 = null!;
    }
}