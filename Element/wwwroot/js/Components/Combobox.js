window.Integrant = window.Integrant || {};
window.Integrant.Element = window.Integrant.Element || {};

window.Integrant.Element.ScrollDropdownToSelection = window.Integrant.Element.ScrollDropdownToSelection || function (elementRef) {
    const dropdownElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Dropdown']");
    const selected = dropdownElem.querySelector("[data-selected]");

    let cTop = dropdownElem.scrollTop;
    let eTop = dropdownElem.offsetTop;

    let padding = selected.clientHeight * 3;

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

    const inputElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Input'] > input");
    const dropdownElem = elementRef.querySelector("div[class~='Integrant.Element.Component.Combobox.Dropdown']");

    const popper = Popper.createPopper(inputElem, dropdownElem, {
        placement: "bottom",
    });

    //
    
    let hasMovedToTop = false;

    const visibilityObserver = new MutationObserver((r) => {
        r.forEach((m) => {
            if (m.type === "attributes" && m.attributeName === "data-shown") {
                popper.update();

                if (!hasMovedToTop) {
                    dropdownElem.scrollTop = 0;
                    // console.log(dropdownElem);
                    hasMovedToTop = true;
                }
            }
        });
    });

    visibilityObserver.observe(dropdownElem, {
        attributes: true,
        attributeFilter: ["data-shown"],
    });

    const focusedOptionObserver = new MutationObserver((r) => {
        for (let i = 0; i < r.length; i++) {
            let m = r[i];

            if (m.type === "attributes" && m.attributeName === "data-focused") {
                let focusedOption = dropdownElem.querySelector("[data-focused]");
                if (focusedOption != null) {
                    ensureInView(dropdownElem, focusedOption);
                }
                break;
            }
        }
    });

    focusedOptionObserver.observe(dropdownElem, {
        attributes: true,
        subtree: true,
    });
    
    //

    inputElem.addEventListener("click", () => {
        if (inputElem.hasAttribute("data-has-selection")) {
            inputElem.setSelectionRange(0, inputElem.value.length); // Select all text in input
        }
    });

};