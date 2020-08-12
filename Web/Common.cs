using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Integrant.Fundament.Structure;
using Integrant.Rudiment.Inputs;

namespace Integrant.Web
{
    public static class Common
    {
        public static readonly Structure<Pages.Index.User> Structure;

        static Common()
        {
            Structure = new Structure<Pages.Index.User>(validator: (structure, value) =>
            {
                // if (DoSlow) Thread.Sleep(200);
                return new List<Validation>
                {
                    new Validation(ValidationResultType.Warning, "Overall validation"),
                };
            });

            Structure.Register(new Member<Pages.Index.User, bool>
            (
                nameof(Pages.Index.User.Boolean),
                (               v, m) => v.Boolean,
                onValueUpdate: (v, m, mv) => v.Boolean = mv,
                input: () => new CheckboxInput<Pages.Index.User>(),
                inputIsRequired: (v, m) => true,
                inputDebounceMilliseconds: 1
            ));

            Structure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.CreatedBy),
                (               v, m) => v.CreatedBy,
                onValueUpdate: (v, m, mv) => v.CreatedBy = mv,
                isVisible: (    v, m) => v.Boolean,
                input: () => new StringInput<Pages.Index.User>(),
                inputIsRequired: (v, m) => true,
                considerDefaultNull: true
            ));

            Structure.Register(new Member<Pages.Index.User, int>(
                nameof(Pages.Index.User.UserID),
                (v, m) => v.UserID,
                // displayValue: ( v, m) => $"[{v.UserID}]",
                key: (          v, m) => "User ID",
                onValueUpdate: (v, m, mv) => v.UserID = mv,
                input: () => new NumberInput<Pages.Index.User, int>(),
                considerDefaultNull: true,
                validator: (v, m) =>
                {
                    var result = new List<Validation>
                    {
                        new Validation(ValidationResultType.Warning, "Warning"),
                        new Validation(ValidationResultType.Valid,   "Is valid"),
                        new Validation(ValidationResultType.Warning,
                            "Some kinda long validation text with type ValidationResultType.Warning"),
                    };
                    if (string.IsNullOrEmpty(v.Name) || v.Name == "A.J.")
                        result.Add(new Validation(ValidationResultType.Invalid, "Is invalid"));

                    return result;
                }));

            Structure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.Name),
                (v, m) => v.Name,
                input: () => new StringInput<Pages.Index.User>(),
                onValueUpdate: (v, m, mv) => v.Name = mv,
                defaultValue: (         v, m) => "A.J. <default>",
                inputIsRequired: (      v, m) => true,
                inputMeetsRequirement: (v, m) => v.Name?.Length > 3,
                validator: (            v, m) => Validation.One(ValidationResultType.Warning, "Warning")
            ));

            Structure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.PhoneNumber),
                (               v, m) => v.PhoneNumber,
                key: (          v, m) => "Phone number",
                isVisible: (    v, m) => v.Name?.Length > 0,
                onValueUpdate: (v, m, mv) => v.PhoneNumber = mv
            ));

            Structure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.Email),
                (v, m) => v.Email,
                input: () => new StringInput<Pages.Index.User>(textArea: true, monospace: true,
                    textAreaCols: (v, m, i) =>
                    {
                        int[] lines = v.Email.Split('\n').Select(l => l.Length).ToArray();
                        return Math.Min(lines.Max() + 5, 60);
                    },
                    textAreaRows: (v, m, i) => v.Email.Split('\n').Length + 1
                ),
                onValueUpdate: (v, m, mv) => v.Email = mv.TrimEnd('!') + "!",
                inputDebounceMilliseconds: 500,
                inputIsDisabled: (v, m) => v.UserID == 1,
                validator: (v, m) =>
                {
                    // Thread.Sleep(500000);
                    return string.IsNullOrEmpty(v.Email)
                        ? Validation.One(ValidationResultType.Warning, "Email is recommended.")
                        : v.Email.Contains("@")
                            ? Validation.One(ValidationResultType.Valid,   "Valid")
                            : Validation.One(ValidationResultType.Invalid, "Invalid");
                }));

            Structure.Register(new Member<Pages.Index.User, DateTime>(
                nameof(Pages.Index.User.StartDate),
                (               v, m) => v.StartDate ?? default,
                onValueUpdate: (v, m, mv) => v.StartDate = mv == default ? new DateTime?() : mv,
                validator: (v, m) =>
                    v.StartDate > DateTime.Now
                        ? Validation.One(ValidationResultType.Invalid, "Start date is in the future.")
                        : Validation.One(ValidationResultType.Valid,   "Valid"),
                input: () => new DateInput<Pages.Index.User>()
            ));

            Structure.Register(new Member<Pages.Index.User, DateTime>(
                nameof(Pages.Index.User.StartTime),
                (               v, m) => v.StartTime,
                onValueUpdate: (v, m, mv) => v.StartTime = mv,
                input: () => new TimeInput<Pages.Index.User>(),
                inputIsDisabled: (v, m) => v.UserID == 1
            ));

            Structure.Register(new Member<Pages.Index.User, DateTime>(
                nameof(Pages.Index.User.CompositeDateTime),
                (               v, m) => v.CompositeDateTime ?? default,
                onValueUpdate: (v, m, mv) => v.CompositeDateTime = mv.Date == default ? new DateTime?() : mv,
                input: () => new DateTimeInput<Pages.Index.User>(),
                inputIsDisabled: (v, m) => v.UserID == 1
            ));

            Structure.Register(new Member<Pages.Index.User, List<string>>(
                nameof(Pages.Index.User.Tags),
                (               v, m) => v.Tags,
                displayValue: ( v, m) => v.Tags != null ? string.Join(" + ", v.Tags) : "<null>",
                onValueUpdate: (v, m, mv) => v.Tags = mv
            ));

            Structure.Register(new Member<Pages.Index.User, int>(
                nameof(Pages.Index.User.DepartmentID),
                (               v, m) => v.DepartmentID,
                onValueUpdate: (v, m, mv) => v.DepartmentID = mv,
                input: () => new SelectInput<Pages.Index.User, int>(),
                selectInputOptions: (v, m) => new List<IOption<int>>
                {
                    new Option<int>("1", 1, "One"),
                    new Option<int>("2", 2, "Two"),
                    new Option<int>("3", 3, "Three", true),
                    new Option<int>("4", 4, "Four"),
                }
            ));
            Structure.Register(new Member<Pages.Index.User, string>(
                nameof(Pages.Index.User.DepartmentType),
                (               v, m) => v.DepartmentType,
                onValueUpdate: (v, m, mv) => v.DepartmentType = mv,
                input: () => new SelectInput<Pages.Index.User, string>(),
                selectInputOptions: (v, m) => new List<IOption<string>>
                {
                    new Option<string>("One",   "One",   "One"),
                    new Option<string>("Two",   "Two",   "Two"),
                    new Option<string>("Three", "Three", "Three"),
                }
            ));

            Structure.Register(new Member<Pages.Index.User, ushort>(
                nameof(Pages.Index.User.DepartmentStatus),
                (               v, m) => v.DepartmentStatus,
                onValueUpdate: (v, m, mv) => v.DepartmentStatus = mv,
                input: () => new SelectInput<Pages.Index.User, ushort>(),
                selectInputOptions: (v, m) => new List<IOption<ushort>>
                {
                    new Option<ushort>("1", 1, "One"),
                    new Option<ushort>("2", 2, "Two"),
                    new Option<ushort>("3", 3, "Three"),
                }
            ));
        }
    }
}