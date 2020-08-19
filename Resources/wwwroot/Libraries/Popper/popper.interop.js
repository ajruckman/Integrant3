window.Integrant = window.Integrant || {};
window.Integrant.Resources = window.Integrant.Resources || {};

const showEvents = ["focus"];
const hideEvents = ["blur"];

window.Integrant.Resources.ScrollTop = window.Integrant.Resources.ScrollTop || function (tooltipElem) {
    const selected = tooltipElem.querySelector("[data-selected]");
    console.log(selected);
    console.log(tooltipElem.scrollTop);
    console.log(selected.offsetTop);

    tooltipElem.scrollTop = selected.offsetTop;
};

// https://gomakethings.com/finding-the-next-and-previous-sibling-elements-that-match-a-selector-with-vanilla-js/
function getNextSibling(elem, selector) {

    // Get the next sibling element
    var sibling = elem.nextElementSibling;

    // If the sibling matches our selector, use it
    // If not, jump to the next sibling and continue the loop
    while (sibling) {
        if (sibling.matches(selector)) return sibling;
        sibling = sibling.nextElementSibling;
    }
}

function getPreviousSibling(elem, selector) {

    // Get the next sibling element
    var sibling = elem.previousElementSibling;

    // If there's no selector, return the first sibling
    if (!selector) return sibling;

    // If the sibling matches our selector, use it
    // If not, jump to the next sibling and continue the loop
    while (sibling) {
        if (sibling.matches(selector)) return sibling;
        sibling = sibling.previousElementSibling;
    }

}

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
    
    // //Check if out of view
    // if (eTop < cTop) {
    //     container.scrollTop -= (cTop - eTop);
    // } else if (eBottom > cBottom) {
    //     container.scrollTop += (eBottom - cBottom);
    // }
}

window.Integrant.Resources.EnsureOptionInView = window.Integrant.Resources.EnsureOptionInView || function (referenceElem, tooltipElem) {
    let focusedOption = tooltipElem.querySelector("[data-focused]");
    if (focusedOption != null) {
        console.log(focusedOption);
        ensureInView(tooltipElem, focusedOption);
    }
};

window.Integrant.Resources.CreatePopper = window.Integrant.Resources.CreatePopper || function (controller, referenceElem, tooltipElem, placement) {
    console.log(referenceElem);
    console.log(tooltipElem);
    console.log(placement);
    

    let focusedOption = null;

    const popper = Popper.createPopper(referenceElem, tooltipElem, {
        placement: placement,
    });

    referenceElem.addEventListener("click", () => {
        console.log("click");
        if (referenceElem.hasAttribute("data-has-selection")) {
            console.log(this);
            referenceElem.setSelectionRange(0, referenceElem.value.length); // Select all text in input
            // referenceElem.select();
        }
    });

    referenceElem.addEventListener("keyup", (e) => {
        setTimeout(() => {
            let focusedOption = tooltipElem.querySelector("[data-focused]");
            if (focusedOption != null) {
                console.log(focusedOption);
                ensureInView(tooltipElem, focusedOption);
            }
        }, 0);
    });

    // referenceElem.addEventListener("keydown", (e) => {
    //     if (e.key === "Enter" && focusedOption != null) {
    //         controller.invokeMethodAsync('SelectFromJS', focusedOption.getAttribute('data-i'))
    //     }
    // })
    
    // referenceElem.addEventListener("keydown", (e) => {
    //     console.log(e);
    //
    //     if (focusedOption != null) {
    //         focusedOption.removeAttribute('data-focused');
    //     }
    //
    //     const first = tooltipElem.querySelector(".Option:not([hidden]):first-child");
    //     if (first == null) {
    //         focusedOption = null;
    //         return;
    //     }
    //    
    //     switch (e.key) {
    //         case "ArrowUp":
    //             if (focusedOption == null) {
    //                 console.log("setting to first");
    //                 focusedOption = first;
    //             } else if (focusedOption === first) {
    //             } else {
    //                 const previous = getPreviousSibling(focusedOption, ":not([hidden])");
    //                 if (previous != null) {
    //                     console.log("setting to previous:");
    //                     console.log(focusedOption);
    //
    //                     focusedOption = previous;
    //                 }
    //             }
    //             break;
    //         case "ArrowDown":
    //             if (focusedOption == null) {
    //                 console.log("setting to first");
    //                 focusedOption = first;
    //             } else {
    //                 const next = getNextSibling(focusedOption, ":not([hidden])");
    //                 if (next != null) {
    //                     console.log("setting to next:");
    //                     console.log(next);
    //
    //                     focusedOption = next;
    //                 }
    //             }
    //             break;
    //         case "Enter":
    //             if (focusedOption != null) {
    //                 controller.invokeMethodAsync('SelectFromJS', focusedOption.getAttribute('data-i'))
    //             }
    //             break;
    //     }
    //
    //     if (focusedOption != null) {
    //         focusedOption.setAttribute('data-focused', '');
    //
    //         ensureInView(tooltipElem, focusedOption);
    //         setTimeout(() => {
    //             focusedOption.focus();
    //         }, 0);
    //     }
    //
    //     console.log(document.activeElement);
    //
    //     // if (e.key === "Tab") {
    //     //     e.stopPropagation();
    //     // }
    // });

    let hasMovedToTop = false;
    
    function show(e) {
        console.log("show");
        setTimeout(() => {
            // Prevent remembering scroll position in dropdown on page load
            if (!hasMovedToTop) {
                tooltipElem.scrollTop = 0;
                hasMovedToTop = true;
            }
            popper.update();
        }, 10);


        // // tooltipElem.removeAttribute('hidden');
        // console.log("update");
        // console.log(tooltipElem.hasAttribute("data-shown"));
        // console.log(tooltipElem.getAttribute("data-shown"));
        //
        // setTimeout(function () {
        //     popper.update();
        // }, 0);
    }

    function hide(e) {
        setTimeout(() => {

            console.log("hide");
            // tooltipElem.removeAttribute("data-shown");
        });
        // tooltipElem.setAttribute('hidden', 'hidden');
    }

    showEvents.forEach(event => {
        referenceElem.addEventListener(event, show);
    });

    hideEvents.forEach(event => {
        referenceElem.addEventListener(event, hide);
    });

};