window.Integrant = window.Integrant || {};
window.Integrant.Element = window.Integrant.Element || {};

window.Integrant.Element.ScrollDropdownToSelection = window.Integrant.Element.ScrollDropdownToSelection || function (elementRef) {
    const dropdownElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Dropdown']");
    const selected = dropdownElem.querySelector("[data-selected]");

    let cTop = dropdownElem.scrollTop;
    let eTop = dropdownElem.offsetTop;

    let padding = selected.clientHeight * 3;

    // console.log("Scroll to selection:");
    // console.log(selected);

    dropdownElem.scrollTop = Math.min(selected.offsetTop - padding, dropdownElem.scrollHeight);
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

window.Integrant.Element.CreateCombobox = window.Integrant.Element.CreateCombobox || function (elementRef) {

    console.log(elementRef);
    
    const inputElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Input'] > input");
    const dropdownElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Dropdown']");

    const popper = Popper.createPopper(inputElem, dropdownElem, {
        placement: "bottom-start",
    });

    //

    let hasMovedToTop = false;

    const visibilityObserver = new MutationObserver((r) => {
        for (const m of r) {
            if (m.type === "attributes" && m.attributeName === "data-shown") {
                popper.update();

                if (!hasMovedToTop) {
                    // console.log("Scroll to top");
                    dropdownElem.scrollTop = 0;
                    hasMovedToTop = true;
                }
            }
        }
    });

    visibilityObserver.observe(dropdownElem, {
        attributes: true,
        attributeFilter: ["data-shown"],
    });

    const focusedOptionObserver = new MutationObserver((r) => {
        for (const m of r) {
            if (m.type === "attributes" && m.attributeName === "data-focused") {
                let focusedOption = dropdownElem.querySelector("[data-focused]");
                if (focusedOption != null) {
                    // console.log("Ensure in view:");
                    // console.log(focusedOption);
                    ensureInView(dropdownElem, focusedOption);
                }
                break;
            }
        }
    });

    focusedOptionObserver.observe(dropdownElem, {
        attributes: true,
        attributeFilter: ["data-focused"],
        subtree: true,
    });

    //

    inputElem.addEventListener("click", () => {
        if (inputElem.hasAttribute("data-has-selection")) {
            inputElem.setSelectionRange(0, inputElem.value.length); // Select all text in input
        }
    });

};