using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

// ReSharper disable BuiltInTypeReferenceStyleForMemberAccess

namespace Integrant.Rudiment.Inputs
{
    public class NumberInput<TStructure, TMember> : IParsableInput<TStructure, TMember, string>
    {
        private readonly int?    _min;
        private readonly int?    _max;
        private readonly double? _step;

        public NumberInput
        (
            int?                                 min    = null,
            int?                                 max    = null,
            double?                              step   = null,
            Parser<TStructure, TMember, string>? parser = null
        )

        {
            _min   = min;
            _max   = max;
            _step  = step;
            Parser = parser ?? DefaultParser;
        }

        public event Action<TStructure, TMember>? OnInput;

        public void Reset() { }

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            var classes = new ClassSet(
                "Integrant.Element.Input",
                "Integrant.Rudiment.Input." + nameof(NumberInput<TStructure, TMember>)
            );

            bool required = InputBuilder.Required(builder, ref seq, structure, value, member, classes);
            bool disabled = InputBuilder.Disabled(builder, ref seq, structure, value, member, classes);

            if (member.InputPlaceholder != null)
                builder.AddAttribute(++seq, "placeholder",
                    member.InputPlaceholder.Invoke(value, member));

            builder.AddAttribute(++seq, "class", classes.Format());

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                member,
                "input", "number",
                "value", member.InputValue.Invoke(value, member),
                required, disabled,
                args => OnChange(value, member, args)
            );

            if (_min != null)
                builder.AddAttribute(++seq, "min", _min.Value);

            if (_max != null)
                builder.AddAttribute(++seq, "max", _max.Value);

            if (_step != null)
                builder.AddAttribute(++seq, "step", _step.Value);

            InputBuilder.CloseInnerInput(builder);

            builder.CloseElement();
        };

        public event Action<TStructure, string>? OnRawInput;

        public Parser<TStructure, TMember, string> Parser { get; }

        private void OnChange(TStructure value, Member<TStructure, TMember> member, ChangeEventArgs args)
        {
            string s = args.Value?.ToString() ?? "";

            OnRawInput?.Invoke(value, s);
            OnInput?.Invoke(value, Parser.Invoke(value, member, s));
        }

        private static TMember DefaultParser(TStructure value, Member<TStructure, TMember> member, string raw)
        {
            if (raw == "") return default!;

            return (TMember) (object) (Type.GetTypeCode(typeof(TMember)) switch
            {
                TypeCode.Byte    => Byte.Parse(raw),
                TypeCode.Int16   => Int16.Parse(raw),
                TypeCode.UInt16  => UInt16.Parse(raw),
                TypeCode.Int32   => Int32.Parse(raw),
                TypeCode.UInt32  => UInt32.Parse(raw),
                TypeCode.Int64   => Int64.Parse(raw),
                TypeCode.UInt64  => UInt64.Parse(raw),
                TypeCode.Single  => Single.Parse(raw),
                TypeCode.Double  => Double.Parse(raw),
                TypeCode.Decimal => Decimal.Parse(raw),
                _                => throw new ArgumentOutOfRangeException(),
            });
        }
    }
}