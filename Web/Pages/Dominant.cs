using System;
using Integrant.Dominant;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Integrant.Web.Pages
{
    public partial class Dominant
    {
        private HTMLTextInput _textInput;
        private HTMLDateInput _dateInput;

        [Inject] public IJSRuntime JSRuntime { get; set; }

        protected override void OnInitialized()
        {
            _textInput = new HTMLTextInput(JSRuntime);
            _textInput.OnChange += v => Console.WriteLine($"Value: {v}");

            _dateInput = new HTMLDateInput(JSRuntime);
            _dateInput.OnChange += v => Console.WriteLine($"Value: {v}");
        }
    }
}