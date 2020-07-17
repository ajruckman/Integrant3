using System;
using Microsoft.AspNetCore.Components;

namespace Fundament.Input
{
    public interface IInput<TStructure, TMember>
    {
        public event Action<TStructure, TMember> OnInput;

        public RenderFragment Render
        (
            Structure<TStructure> structure, TStructure value, Member<TStructure, TMember> member
        );
    }
}