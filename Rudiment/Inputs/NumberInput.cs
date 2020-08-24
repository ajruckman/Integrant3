using System;
using System.Collections.Generic;
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
            _min  = min;
            _max  = max;
            _step = step;

            if (parser == null && !SupportedTypeCodes.Contains(Type.GetTypeCode(typeof(TMember))))
            {
                throw new ArgumentException(
                    $"NumberInput cannot convert input type '{typeof(string)}' "     +
                    $"to Member type '{typeof(TMember)}' using its default Parser, " +
                    $"and no custom Parser was provided.");
            }

            Parser = parser ?? DefaultParser;
        }

        public event Action<TStructure, TMember>? OnInput;

        public void Reset(StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TMember> member) { }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, TMember> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            //

            var classes = new ClassSet(
                "Integrant.Element.Override.Input",
                "Integrant.Rudiment.Input." + nameof(NumberInput<TStructure, TMember>)
            );

            bool required = InputBuilder.Required(value, member.Member, classes);
            bool disabled = InputBuilder.Disabled(value, member.Member, classes);

            builder.AddAttribute(++seq, "class", classes.ToString());

            //

            InputBuilder.OpenInnerInput
            (
                builder, ref seq,
                value, member.Member,
                "input", "number",
                "value", member.Member.InputValue.Invoke(value, member.Member),
                required, disabled,
                args => OnChange(value, member.Member, args)
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

        private static readonly HashSet<TypeCode> SupportedTypeCodes = new HashSet<TypeCode>
        {
            TypeCode.Byte,
            TypeCode.Int16,
            TypeCode.UInt16,
            TypeCode.Int32,
            TypeCode.UInt32,
            TypeCode.Int64,
            TypeCode.UInt64,
            TypeCode.Single,
            TypeCode.Double,
            TypeCode.Decimal,
        };

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