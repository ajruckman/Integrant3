using System;
using Microsoft.AspNetCore.Components;

namespace Integrant.Fundament.Structure
{
    public interface IInput<TStructure, TMember>
    {
        public void Reset();

        public RenderFragment Render
        (
            StructureInstance<TStructure>       structure,
            TStructure                          value,
            MemberInstance<TStructure, TMember> member
        );

        public event Action<TStructure, TMember>? OnInput;
    }

    public interface IParsableInput<TStructure, TMember, TRaw> : IInput<TStructure, TMember>
    {
        public event Action<TStructure, TRaw>? OnRawInput;

        public Parser<TStructure, TMember, TRaw> Parser { get; }
    }

    public delegate TMember Parser<TStructure, TMember, in TRaw>
        (TStructure value, Member<TStructure, TMember> member, TRaw raw);

    // public interface IInput<TStructure, TMember, TInput> : IInput<TStructure, TMember>
    // {
    //     public event Action<TStructure, TInput>? OnRawInput;
    //
    //     public delegate TMember Parser(TInput from);
    // }
}