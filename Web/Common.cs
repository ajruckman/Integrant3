using System;
using System.Collections.Generic;
using System.Linq;
using Integrant.Fundament.Structure;
using Integrant.Rudiment.Inputs;

namespace Integrant.Web
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
        public int          DepartmentID      { get; set; }
        public string       DepartmentType    { get; set; }
        public ushort       DepartmentStatus  { get; set; }
        public TimeSpan     TimeSpan          { get; set; }
        public double       Double            { get; set; }
    }

    public static class Common
    {
        public static readonly Structure<User> Structure;

        static Common()
        {
            Structure = new Structure<User>(validator: (structure, value) =>
            {
                // if (DoSlow) Thread.Sleep(200);
                return new List<Validation>
                {
                    new Validation(ValidationResultType.Warning, "Overall validation"),
                };
            });

            Structure.Register(new Member<User, bool>
            (
                nameof(User.Boolean),
                (v,                m) => v.Boolean,
                valueUpdater: (v,  m, mv) => v.Boolean = mv,
                input: () => new CheckboxInput<User>(),
                inputIsRequired: (v, m) => true,
                inputDebounceMilliseconds: 1
            ));

            Structure.Register(new Member<User, string>(
                nameof(User.CreatedBy),
                (v,                m) => v.CreatedBy,
                valueUpdater: (v,  m, mv) => v.CreatedBy = mv,
                onValueUpdate: (v, m, mv) => Console.WriteLine($"++ {mv}"),
                isVisible: (v,     m) => v.Boolean,
                input: () => new StringInput<User>(),
                inputIsRequired: (v, m) => true,
                considerDefaultNull: true
            ));

            Structure.Register(new Member<User, int>(
                nameof(User.UserID),
                (v, m) => v.UserID,
                // displayValue: ( v, m) => $"[{v.UserID}]",
                key: (v,          m) => "User ID",
                valueUpdater: (v, m, mv) => v.UserID = mv,
                input: () => new NumberInput<User, int>(),
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
                        result.Add(new Validation(ValidationResultType.Invalid,
                            "Is invalid"));

                    return result;
                }));

            Structure.Register(new Member<User, string>(
                nameof(User.Name),
                (v, m) => v.Name,
                input: () => new StringInput<User>(),
                valueUpdater: (v,          m, mv) => v.Name = mv,
                defaultValue: (v,          m) => "A.J. <default>",
                inputIsRequired: (v,       m) => true,
                inputMeetsRequirement: (v, m) => v.Name?.Length > 3,
                validator: (v, m) =>
                    Validation.One(ValidationResultType.Warning, "Warning")
            ));

            Structure.Register(new Member<User, string>(
                nameof(User.PhoneNumber),
                (v,               m) => v.PhoneNumber,
                key: (v,          m) => "Phone number",
                isVisible: (v,    m) => v.Name?.Length > 0,
                valueUpdater: (v, m, mv) => v.PhoneNumber = mv
            ));

            Structure.Register(new Member<User, string>(
                nameof(User.Email),
                (v, m) => v.Email,
                input: () =>
                    new StringInput<User>(textArea: true, monospace: true,
                        textAreaCols: (v, m, i) =>
                        {
                            int[] lines = v.Email.Split('\n')
                                           .Select(l => l
                                               .Length)
                                           .ToArray();
                            return Math.Min(lines.Max() + 5,
                                60);
                        },
                        textAreaRows: (v, m, i) =>
                            v.Email.Split('\n').Length + 1
                    ),
                valueUpdater: (v, m, mv) => v.Email = mv.TrimEnd('!') + "!",
                inputDebounceMilliseconds: 500,
                inputIsDisabled: (v, m) => v.UserID == 1,
                validator: (v, m) =>
                {
                    // Thread.Sleep(500000);
                    return string.IsNullOrEmpty(v.Email)
                        ? Validation.One(ValidationResultType.Warning,
                            "Email is recommended.")
                        : v.Email.Contains("@")
                            ? Validation.One(ValidationResultType.Valid,
                                "Valid")
                            : Validation.One(ValidationResultType.Invalid,
                                "Invalid");
                }));

            Structure.Register(new Member<User, DateTime>(
                nameof(User.StartDate),
                (v, m) => v.StartDate ?? default,
                valueUpdater: (v, m, mv) =>
                    v.StartDate = mv == default ? new DateTime?() : mv,
                validator: (v, m) =>
                    v.StartDate > DateTime.Now
                        ? Validation.One(ValidationResultType.Invalid,
                            "Start date is in the future.")
                        : Validation.One(ValidationResultType.Valid, "Valid"),
                input: () => new DateInput<User>()
            ));

            Structure.Register(new Member<User, DateTime>(
                nameof(User.StartTime),
                (v,               m) => v.StartTime,
                valueUpdater: (v, m, mv) => v.StartTime = mv,
                input: () => new TimeInput<User>(),
                inputIsDisabled: (v, m) => v.UserID == 1
            ));

            Structure.Register(new Member<User, DateTime>(
                nameof(User.CompositeDateTime),
                (v, m) => v.CompositeDateTime ?? default,
                valueUpdater: (v, m, mv) =>
                    v.CompositeDateTime =
                        mv.Date == default ? new DateTime?() : mv,
                input: () => new DateTimeInput<User>(),
                inputIsDisabled: (v, m) => v.UserID == 1
            ));

            Structure.Register(new Member<User, List<string>>(
                nameof(User.Tags),
                (v, m) => v.Tags,
                displayValue: (v, m) =>
                    v.Tags != null
                        ? string.Join(" + ", v.Tags)
                        : "<null>",
                valueUpdater: (v, m, mv) => v.Tags = mv
            ));

            Structure.Register(new Member<User, int>(
                nameof(User.DepartmentID),
                (v,               m) => v.DepartmentID,
                valueUpdater: (v, m, mv) => v.DepartmentID = mv,
                input: () => new SelectInput<User, int>(),
                selectInputOptions: (v, m) => new List<IOption<int>>
                {
                    new Option<int>("1", 1, "One"),
                    new Option<int>("2", 2, "Two"),
                    new Option<int>("3", 3, "Three", true),
                    new Option<int>("4", 4, "Four"),
                }
            ));

            Structure.Register(new Member<User, string>(
                nameof(User.DepartmentType),
                (v,               m) => v.DepartmentType,
                valueUpdater: (v, m, mv) => v.DepartmentType = mv,
                input: () => new SelectInput<User, string>(),
                selectInputOptions: (v, m) => new List<IOption<string>>
                {
                    new Option<string>("One",   "One",   "One"),
                    new Option<string>("Two",   "Two",   "Two"),
                    new Option<string>("Three", "Three", "Three"),
                }
            ));

            Structure.Register(new Member<User, ushort>(
                nameof(User.DepartmentStatus),
                (v,               m) => v.DepartmentStatus,
                valueUpdater: (v, m, mv) => v.DepartmentStatus = mv,
                input: () => new SelectInput<User, ushort>(),
                selectInputOptions: (v, m) => new List<IOption<ushort>>
                {
                    new Option<ushort>("1", 1, "One"),
                    new Option<ushort>("2", 2, "Two"),
                    new Option<ushort>("3", 3, "Three"),
                }
            ));

            Structure.Register(new Member<User, TimeSpan>
            (
                id: nameof(User.TimeSpan),
                value: (v,        m) => v.TimeSpan,
                valueUpdater: (v, m, mv) => v.TimeSpan = mv,
                defaultValue: (v, m) => TimeSpan.FromDays(7),
                input: () => new NumberInput<User, TimeSpan>(
                    parser: (v, m, mv) => mv != ""
                        ? TimeSpan.FromDays(int.Parse(mv))
                        : TimeSpan.Zero),
                inputValue: (v,   m) => v.TimeSpan.TotalDays,
                displayValue: (v, m) => $"{v.TimeSpan.TotalDays} days"
            ));

            Structure.Register(new Member<User, double>
            (
                nameof(User.Double),
                (v,               m) => v.Double,
                valueUpdater: (v, m, mv) => v.Double = mv,
                input: () => new NumberInput<User, double>(step: 0.01)
            ));
        }
    }
}