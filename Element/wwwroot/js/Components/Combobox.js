window.Integrant = window.Integrant || {};
window.Integrant.Element = window.Integrant.Element || {};

window.Integrant.Element.ScrollDropdownToSelection = window.Integrant.Element.ScrollDropdownToSelection || function (elementRef) {
    const dropdownElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Dropdown']");
    const selected = dropdownElem.querySelector("[data-selected]");

    let cTop = dropdownElem.scrollTop;
    let eTop = dropdownElem.offsetTop;

    let padding = selected.clientHeight * 3;

    dropdownElem.scrollTop = Math.min(selected.offsetTop - padding, dropdownElem.scrollHeight)
};

// https://stackoverflow.com/a/37285344/9911189
function ensureInView(container, element) {

    //Determine container top and bottom
    let cTop = container.scrollTop;
    let cBottom = cTop + container.clientHeight;

    //Determine element top and bottom
    let eTop = element.offsetTop;
    let eBottom = eTop + element.clientHeight;

    let padding = element.clientHeight * 3;

    //Check if out of view
    if ((eTop - padding) < cTop) {
        container.scrollTop -= Math.max((cTop - eTop) + padding, 0);
    } else if ((eBottom + padding) > cBottom) {
        container.scrollTop += Math.min((eBottom - cBottom) + padding, cBottom);
    }
}

// window.Integrant.Element.EnsureOptionInView = window.Integrant.Element.EnsureOptionInView || function (referenceElem, tooltipElem) {
//     let focusedOption = tooltipElem.querySelector("[data-focused]");
//     if (focusedOption != null) {
//         console.log(focusedOption);
//         ensureInView(tooltipElem, focusedOption);
//     }
// };

window.Integrant.Element.CreateCombobox = window.Integrant.Element.CreateCombobox || function (elementRef) {

    const inputElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Input'] > input");
    const dropdownElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Dropdown']");

    const popper = Popper.createPopper(inputElem, dropdownElem, {
        placement: "bottom",
    });

    inputElem.addEventListener("click", () => {
        // console.log("click");
        if (inputElem.hasAttribute("data-has-selection")) {
            // console.log(this);
            inputElem.setSelectionRange(0, inputElem.value.length); // Select all text in input
            // referenceElem.select();
        }
    });

    inputElem.addEventListener("keyup", (e) => {
        setTimeout(() => {
            let focusedOption = dropdownElem.querySelector("[data-focused]");
            if (focusedOption != null) {
                // console.log(focusedOption);
                ensureInView(dropdownElem, focusedOption);
            }
        }, 0);
    });

    let hasMovedToTop = false;


    inputElem.addEventListener("focus", (e) => {
        // console.log("focus");
        setTimeout(() => {
            // Prevent remembering scroll position in dropdown on page load
            if (!hasMovedToTop) {
                dropdownElem.scrollTop = 0;
                // console.log(dropdownElem);
                hasMovedToTop = true;
            }
            popper.update();
        }, 10);
    });

};