using System;
using Microsoft.AspNetCore.Components;

namespace Fundament
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