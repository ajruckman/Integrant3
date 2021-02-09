using System;
using System.Security.Cryptography;
using Integrant.Dominant;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Integrant.Web.Pages
{
    public partial class Dominant
    {
        private HTMLTextInput _textInput = null!;
        private HTMLDateInput _dateInput = null!;

        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

        protected override void OnInitialized()
        {
            _textInput = new HTMLTextInput
            (
                JSRuntime,
                RandomNumberGenerator.GetInt32(15).ToString(),
                false,
                false,
                "placeholder!"
            );
            _textInput.OnChange += v => Console.WriteLine($"Value: {v}");

            _dateInput          =  new HTMLDateInput(JSRuntime);
            _dateInput.OnChange += v => Console.WriteLine($"Value: {v}");
        }
    }
}