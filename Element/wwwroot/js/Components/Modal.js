window.Integrant = window.Integrant || {};
window.Integrant.Element = window.Integrant.Element || {};

window.Integrant.Element.FocusModal = window.Integrant.Element.FocusModal || function (elementRef) {
    setTimeout(() => {
        elementRef.focus();
    }, 100);
}
