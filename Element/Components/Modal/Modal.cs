using System;
using Microsoft.AspNetCore.Components;

namespace Integrant.Element.Components.Modal
{
    public sealed class Modal
    {
        public delegate string MaxWidthGetter();

        private readonly MaxWidthGetter _maxWidthGetter;

        public Modal(MaxWidthGetter? maxWidthGetter = null)
        {
            _maxWidthGetter = maxWidthGetter ?? DefaultWidthGetter;
        }

        internal RenderFragment? Heading         { get; set; }
        internal bool            Shown           { get; private set; }
        public   Action?         StateHasChanged { get; internal set; }

        private static string DefaultWidthGetter() => "600px";
        internal       string MaxWidth()           => _maxWidthGetter.Invoke();

        public void Show()
        {
            Console.WriteLine("Show");
            Shown = true;
            if (StateHasChanged == null) throw new ArgumentNullException(nameof(StateHasChanged));
            StateHasChanged.Invoke();
        }

        public void Hide()
        {
            Console.WriteLine("Hide");
            Shown = false;
            if (StateHasChanged == null) throw new ArgumentNullException(nameof(StateHasChanged));
            StateHasChanged.Invoke();
        }
    }
}