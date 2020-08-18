using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Integrant.Element.Components.Combobox;
using Integrant.Fundament;
using Integrant.Resources.Libraries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Integrant.Web.Pages
{
    public partial class PopperTests
    {
        private Faker<User>        _faker   = null!;
        private List<User>         _users   = null!;
        private List<Option> _options = null!;
        private bool               _shown   = false;

        private User?   _selected   = null;
        private User?   _focused    = null;
        private string? _searchTerm = "";

        public ElementReference UserInput1    { get; set; }
        public ElementReference UserDropdown1 { get; set; }

        private string InputValue()
        {
            return _selected?.Name ?? (_searchTerm ?? "");
        }

        public class Option : IOption<User>
        {
            public string Key           { get; set; }
            public User   Value         { get; set; }
            public string OptionText    { get; set; }
            public string SelectionText { get; set; }
            public bool   Disabled      { get; set; }
        }

        private Combobox<User> _combobox = null!;

        protected override void OnInitialized()
        {
            _faker = new Faker<User>()
                    .RuleFor(u => u.ID,         (f, u) => f.IndexFaker)
                    .RuleFor(u => u.Name,       (f, u) => f.Name.FullName())
                    .RuleFor(u => u.Department, (f, u) => f.Commerce.Department());

            _users = _faker.Generate(100);
            
            _options = _users.Select(v => new Option
            {
                Key           = v.ID.ToString(),
                Value         = v,
                OptionText    = $"{v.Name} - {v.Department}",
                SelectionText = v.Name,
            }).ToList();

            _combobox = new Combobox<User>(JSRuntime, () => _options);
        }

        // protected override void OnAfterRender(bool firstRender)
        // {
        //     if (firstRender)
        //     {
        //         Popper.Create(JSRuntime, DotNetObjectReference.Create(this), UserInput1, UserDropdown1);
        //         _shown = false;
        //         StateHasChanged();
        //     }
        //
        //     // if (_selected != null && _shown)
        //     // {
        //     //     InvokeAsync(() => { JSRuntime.InvokeVoidAsync("Integrant.Resources.ScrollTop", UserDropdown1); });
        //     // }
        // }
        //
        // private IEnumerable<User> FilteredUsers()
        // {
        //     if (_searchTerm == null) return _users;
        //     return _users.Where(v => v.Name.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
        // }
        //
        // private void Select(User user)
        // {
        //     _selected   = user;
        //     _focused    = user;
        //     _searchTerm = null;
        //     Console.WriteLine($"Selection -> {user.ID}");
        //     _shown = false;
        //
        //     if (_selected != null && _shown)
        //     {
        //         InvokeAsync(() => { JSRuntime.InvokeVoidAsync("Integrant.Resources.ScrollTop", UserDropdown1); });
        //     }
        //
        //     StateHasChanged();
        // }
        //
        // private void Select(KeyboardEventArgs args, User user)
        // {
        //     throw new NotImplementedException();
        //     if (args.Key != "Enter") return;
        //     Select(user);
        // }
        //
        // // This is required for 'onmousedown:preventDefault' to work.
        // private static void OnMouseDown() { }
        //
        // private void UpdateFilter(ChangeEventArgs args)
        // {
        //     Console.WriteLine($"Search term -> {_searchTerm}");
        //     var v = args.Value?.ToString();
        //     _searchTerm = string.IsNullOrEmpty(v) ? null : v;
        //     _selected   = null;
        //     _shown      = true;
        // }
        //
        // private void OnClick(MouseEventArgs args)
        // {
        //     if (args.Button != 0) return;
        //     Console.WriteLine("Click");
        //     _shown = true;
        // }
        //
        // [JSInvokable]
        // public void SelectFromJS(string i)
        // {
        //     Console.WriteLine($"Select from JS: {i}");
        //     Select(_users[int.Parse(i)]);
        // }
        //
        // private void OnInputKeyDown(KeyboardEventArgs args)
        // {
        //     Console.WriteLine(args.Key);
        //
        //     // if ((args.Key == "ArrowUp" || args.Key == "ArrowDown") && !_shown)
        //     // {
        //     //     _shown = true;
        //     //     //     }
        //     //     //
        //     //     //     return;
        //     //     // }
        //
        //     if ((args.Key == "ArrowUp" || args.Key == "ArrowDown") && !_shown)
        //     {
        //         _shown = true;
        //     }
        //
        //     List<User> users = FilteredUsers().ToList();
        //
        //     User? first = users.FirstOrDefault();
        //     if (first == null)
        //     {
        //         Console.WriteLine("No first");
        //         return;
        //     }
        //
        //     switch (args.Key)
        //     {
        //         case "ArrowUp":
        //             if (_focused == null)
        //             {
        //                 _focused = first;
        //                 Console.WriteLine($"Setting to first: {_focused.Name}");
        //             }
        //             else if (_focused.ID != first.ID)
        //             {
        //                 int previousIndex = users.FindIndex(v => v.ID == _focused.ID) - 1;
        //                 _focused = users[previousIndex];
        //                 Console.WriteLine($"Setting to previous: {_focused.Name}");
        //             }
        //             else
        //             {
        //                 Console.WriteLine("Focused is first");
        //             }
        //
        //             break;
        //
        //         case "ArrowDown":
        //             if (_focused == null)
        //             {
        //                 _focused = first;
        //                 Console.WriteLine($"Setting to first: {_focused.Name}");
        //             }
        //             else
        //             {
        //                 int nextIndex = users.FindIndex(v => v.ID == _focused.ID) + 1;
        //                 if (nextIndex < users.Count)
        //                 {
        //                     _focused = users[nextIndex];
        //                     Console.WriteLine($"Setting to next: {_focused.Name}");
        //                 }
        //             }
        //
        //             break;
        //
        //         case "Enter":
        //             if (_focused != null)
        //             {
        //                 Select(_focused);
        //             }
        //
        //             break;
        //     }
        //
        //     JSRuntime.InvokeVoidAsync("Integrant.Resources.EnsureOptionInView", UserInput1, UserDropdown1);
        //
        //     // if (args.Key != "Enter") return;
        //     // Console.WriteLine("Input enter");
        //     // _shown = true;
        //     // }
        // }
        //
        // private void OnInputKeyUp(KeyboardEventArgs args)
        // {
        //     return;
        //
        //     if (args.Key == "ArrowUp" || args.Key == "ArrowDown")
        //     {
        //         JSRuntime.InvokeVoidAsync("Integrant.Resources.EnsureOptionInView", UserInput1, UserDropdown1);
        //     }
        // }
        //
        // private void Focus(FocusEventArgs args)
        // {
        //     Console.WriteLine("Focus");
        //     _shown = true;
        // }
        //
        // // TODO: Remove selection on backspace?
        //
        // private void Blur(FocusEventArgs args)
        // {
        //     // Thread.Sleep(100);
        //     Console.WriteLine("Blur");
        //     _shown = false;
        // }

        public class User
        {
            public int    ID         { get; set; }
            public string Name       { get; set; }
            public string Department { get; set; }
        }
    }
}