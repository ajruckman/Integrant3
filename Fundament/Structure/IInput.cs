using System;
using Microsoft.AspNetCore.Components;

namespace Integrant.Fundament.Structure
{
    public interface IInput<TStructure, TMember>
    {
        public void Reset();

        public RenderFragment Render
        (
            Structure<TStructure>       structure,
            TStructure                  value,
            Member<TStructure, TMember> member
        );

        public event Action<TStructure, TMember>? OnInput;
    }

    public interface ITransformableInput<TStructure, TMember, TRaw> : IInput<TStructure, TMember>
    {
        public event Action<TStructure, TRaw>? OnRawInput;
        
        public Transformer<TStructure, TMember, TRaw> Transformer { get; }
    }

    public delegate TMember Transformer<TStructure, TMember, in TRaw>
        (TStructure value, Member<TStructure, TMember> member, TRaw raw);

    // public interface IInput<TStructure, TMember, TInput> : IInput<TStructure, TMember>
    // {
    //     public event Action<TStructure, TInput>? OnRawInput;
    //
    //     public delegate TMember Transformer(TInput from);
    // }
}