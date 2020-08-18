using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Integrant.Resources.Libraries
{
    public class Popper
    {
        public static async Task Create<T>
        (
            IJSRuntime               jsRuntime,
            DotNetObjectReference<T> controller,
            ElementReference         referenceElem, ElementReference tooltipElem,
            Placement                placement = Placement.Bottom
        ) where T : class
        {
            await jsRuntime.InvokeVoidAsync(
                "Integrant.Resources.CreatePopper",
                controller,
                referenceElem, tooltipElem,
                _placementMap[placement]
            );
        }

        public enum Placement
        {
            TopStart,
            Top,
            TopEnd,
            RightStart,
            Right,
            RightEnd,
            BottomStart,
            Bottom,
            BottomEnd,
            LeftStart,
            Left,
            LeftEnd,
        }

        private static readonly Dictionary<Placement, string> _placementMap = new Dictionary<Placement, string>
        {
            {Placement.TopStart, "top-start"},
            {Placement.Top, "top"},
            {Placement.TopEnd, "top-end"},
            {Placement.RightStart, "right-start"},
            {Placement.Right, "right"},
            {Placement.RightEnd, "right-bottom"},
            {Placement.BottomStart, "bottom-start"},
            {Placement.Bottom, "bottom"},
            {Placement.BottomEnd, "bottom-end"},
            {Placement.LeftStart, "left-start"},
            {Placement.Left, "left"},
            {Placement.LeftEnd, "left-end"},
        };
    }
}