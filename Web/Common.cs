using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Integrant.Fundament;
using Integrant.Rudiment.Input;

namespace Integrant.Web
{
    public static class Common
    {
        public static readonly Structure<Pages.Index.User> UserStructure;
        private const          bool                        DoSlow = false;

        static Common()
        {
            UserStructure = new Structure<Pages.Index.User>(validator: (structure, value) =>
            {
                if (DoSlow) Thread.Sleep(200);
                return new List<Validation>
                {
                    new Validation(ValidationResultType.Warning, "Overall validation"),
                };
            });

            UserStructure.Register(new Member<Pages.Index.User, bool>
            (
                nameof(Pages.Index.User.Boolean),
                (s,                v, m) => v.Boolean,
                onValueUpdate: (s, v, m) => s.Boolean = m,
                input: new CheckboxInput<Pages.Index.User>(),
                inputIsRequired: (s, v, m) => true,
                inputDebounceMilliseconds: 1
            ));

            UserStructure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.CreatedBy),
                (s,                v, m) => v.CreatedBy,
                onValueUpdate: (s, v, m) => s.CreatedBy = m,
                isVisible: (s,     v, m) => v.Boolean,
                input: new StringInput<Pages.Index.User>(),
                inputIsRequired: (s, v, m) => true
            ));

            UserStructure.Register(new Member<Pages.Index.User, int>(
                nameof(Pages.Index.User.UserID),
                (s, v, m) => v.UserID,
                // displayValue: (s,  v, m) => $"[{v.Index.UserID}]",
                key: (s,           v, m) => "Index.User ID",
                onValueUpdate: (s, v, m) => s.UserID = m,
                input: new NumberInput<Pages.Index.User>(),
                considerDefaultNull: true,
                validator: (s, v, m) => new List<Validation>
                {
                    new Validation(ValidationResultType.Warning, "Warning"),
                    new Validation(ValidationResultType.Valid,   "Is valid: " + v.UserID),
                    new Validation(ValidationResultType.Invalid, "Is invalid"),
                    new Validation(ValidationResultType.Warning,
                        "Some kinda long validation text with type ValidationResultType.Warning"),
                }
            ));

            UserStructure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.Name),
                (s, v, m) => v.Name,
                input: new StringInput<Pages.Index.User>(),
                onValueUpdate: (s,         v, m) => s.Name = m,
                defaultValue: (s,          v, m) => "A.J. <default>",
                inputIsRequired: (s,       v, m) => true,
                inputMeetsRequirement: (s, v, m) => v.Name?.Length > 3,
                validator: (s,             v, m) => Validation.One(ValidationResultType.Warning, "Warning")
            ));

            UserStructure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.PhoneNumber),
                (s,                v, m) => v.PhoneNumber,
                key: (s,           v, m) => "Phone number",
                isVisible: (s,     v, m) => v.Name?.Length > 0,
                onValueUpdate: (s, v, m) => s.PhoneNumber = m
            ));

            UserStructure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.Email),
                (s, v, m) => v.Email,
                input: new StringInput<Pages.Index.User>(textArea: true, monospace: true,
                    textAreaCols: (s, v, m, i) =>
                    {
                        int[] lines = v.Email.Split('\n').Select(l => l.Length).ToArray();
                        return Math.Min(lines.Max() + 5, 60);
                    },
                    textAreaRows: (s, v, m, i) => v.Email.Split('\n').Length + 1
                ),
                onValueUpdate: (s, v, m) => s.Email = m.TrimEnd('!') + "!",
                inputDebounceMilliseconds: 500,
                inputIsDisabled: (s, v, m) => v.UserID == 1,
                validator: (s, v, m) =>
                {
                    // Thread.Sleep(500000);
                    return string.IsNullOrEmpty(v.Email)
                        ? Validation.One(ValidationResultType.Warning, "Email is recommended.")
                        : v.Email.Contains("@")
                            ? Validation.One(ValidationResultType.Valid,   "Valid")
                            : Validation.One(ValidationResultType.Invalid, "Invalid");
                }));

            UserStructure.Register(new Member<Pages.Index.User, DateTime>(
                nameof(Pages.Index.User.StartDate),
                (s,                v, m) => v.StartDate ?? default,
                onValueUpdate: (s, v, m) => s.StartDate = m == default ? new DateTime?() : m,
                validator: (s, v, m) =>
                    v.StartDate > DateTime.Now
                        ? Validation.One(ValidationResultType.Invalid, "Start date is in the future.")
                        : Validation.One(ValidationResultType.Valid,   "Valid"),
                input: new DateInput<Pages.Index.User>()
            ));

            UserStructure.Register(new Member<Pages.Index.User, DateTime>(
                nameof(Pages.Index.User.StartTime),
                (s,                v, m) => v.StartTime,
                onValueUpdate: (s, v, m) => s.StartTime = m,
                input: new TimeInput<Pages.Index.User>(),
                inputIsDisabled: (s, v, m) => v.UserID == 1
            ));

            UserStructure.Register(new Member<Pages.Index.User, DateTime>(
                nameof(Pages.Index.User.CompositeDateTime),
                (s,                v, m) => v.CompositeDateTime ?? default,
                onValueUpdate: (s, v, m) => s.CompositeDateTime = m.Date == default ? new DateTime?() : m,
                input: new DateTimeInput<Pages.Index.User>(),
                inputIsDisabled: (s, v, m) => v.UserID == 1
            ));

            UserStructure.Register(new Member<Pages.Index.User, List<string>>(
                nameof(Pages.Index.User.Tags),
                (s,                v, m) => v.Tags,
                displayValue: (s,  v, m) => v.Tags != null ? string.Join(" + ", v.Tags) : "<null>",
                onValueUpdate: (s, v, m) => s.Tags = m
            ));

            //

            UserStructure.GetMember<string>("Name").OnInputDebouncedValue +=
                (s, v, m) => Console.WriteLine($"StructureState<Index.User>." + v.ID + " -> " + v);

            UserStructure.GetMember<string>("Email").OnInputDebouncedValue +=
                (s, v, m) => Console.WriteLine($"StructureState<Index.User>." + v.ID + " -> " + v);
        }
    }
}