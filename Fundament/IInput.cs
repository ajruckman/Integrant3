using System;
using Microsoft.AspNetCore.Components;
using Superset.Web.State;

namespace Integrant.Fundament
{
    public interface IInput<TStructure, TMember>
    {
        public event Action<TStructure, TMember> OnInput;

        public RenderFragment Render
        (
            Structure<TStructure>       structure,
            TStructure                  value,
            Member<TStructure, TMember> member,
            UpdateTrigger               resetInput
        );
    }
}