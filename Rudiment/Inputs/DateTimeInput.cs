using System;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Microsoft.AspNetCore.Components;

namespace Integrant.Rudiment.Inputs
{
    public class DateTimeInput<TStructure> : IInput<TStructure, DateTime>
    {
        public event Action<TStructure, DateTime>? OnInput;

        public void Reset(StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, DateTime> member) { }

        private readonly DateInput<TStructure> _dateInput;
        private readonly TimeInput<TStructure> _timeInput;

        private DateTime _dateValue;
        private DateTime _timeValue;

        public DateTimeInput()
        {
            _dateInput = new DateInput<TStructure>();
            _timeInput = new TimeInput<TStructure>();

            _dateInput.OnInput += (structure, time) =>
            {
                _dateValue = time;
                Update(structure);
            };
            _timeInput.OnInput += (structure, time) =>
            {
                _timeValue = time;
                Update(structure);
            };
        }
        
        private void Update(TStructure structure)
        {
            DateTime composite = _dateValue + _timeValue.TimeOfDay;
            OnInput?.Invoke(structure, composite);
        }

        public RenderFragment Render
        (
            StructureInstance<TStructure> structure, TStructure value, MemberInstance<TStructure, DateTime> member
        ) => builder =>
        {
            int seq = -1;

            builder.OpenElement(++seq, "div");

            var classes = new ClassSet(
                "Integrant.Element.Override.Input",
                "Integrant.Element.Override.Input:Composite",
                "Integrant.Rudiment.Input." + nameof(DateTimeInput<TStructure>)
            );

            builder.AddAttribute(++seq, "class", classes.ToString());

            //

            builder.AddContent(++seq, _dateInput.Render(structure, value, member));
            builder.AddContent(++seq, _timeInput.Render(structure, value, member));
            
            builder.CloseElement();
        };
    }
}