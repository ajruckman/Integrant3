using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

// ReSharper disable BuiltInTypeReferenceStyleForMemberAccess

namespace Integrant.Rudiment.Inputs
{
    public class NumberInput<TStructure, TMember> : ITransformableInput<TStructure, TMember, string>
    {
        private readonly int?    _min;
        private readonly int?    _max;
        private readonly double? _step;

        public NumberInput
        (
            int?                                      min         = null,
            int?                                      max         = null,
            double?                                   step        = null,
            Transformer<TStructure, TMember, string>? transformer = null
        )

        {
            _min        = min;
            _max        = max;
            _step       = step;
            Transformer = transformer ?? DefaultTransformer;
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

        public Transformer<TStructure, TMember, string> Transformer { get; }

        private void OnChange(TStructure value, Member<TStructure, TMember> member, ChangeEventArgs args)
        {
            string s = args.Value?.ToString() ?? "";

            // int v = s == ""
            //     ? default
            //     : int.Parse(s);

            OnRawInput?.Invoke(value, s);
            OnInput?.Invoke(value, Transformer.Invoke(value, member, s));

            // if (_transformer == null)
            // OnInput?.Invoke(value, (TMember) (object) v);
            // else
            //     OnInput?.Invoke(value, _transformer.Invoke(v));
        }

        private static TMember DefaultTransformer(TStructure value, Member<TStructure, TMember> member, string raw)
        {
            if (raw == "") return default!;

            switch (Type.GetTypeCode(typeof(TMember)))
            {
                case TypeCode.Byte:
                    return (TMember) (object) Byte.Parse(raw);
                case TypeCode.Int16:
                    return (TMember) (object) Int16.Parse(raw);
                case TypeCode.UInt16:
                    return (TMember) (object) UInt16.Parse(raw);
                case TypeCode.Int32:
                    return (TMember) (object) Int32.Parse(raw);
                case TypeCode.UInt32:
                    return (TMember) (object) UInt32.Parse(raw);
                case TypeCode.Int64:
                    return (TMember) (object) Int64.Parse(raw);
                case TypeCode.UInt64:
                    return (TMember) (object) UInt64.Parse(raw);
                case TypeCode.Single:
                    return (TMember) (object) Single.Parse(raw);
                case TypeCode.Double:
                    return (TMember) (object) Double.Parse(raw);
                case TypeCode.Decimal:
                    return (TMember) (object) Decimal.Parse(raw);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}