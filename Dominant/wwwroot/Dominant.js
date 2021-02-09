window.Integrant = window.Integrant || {};
window.Integrant.Dominant = window.Integrant.Dominant || {};

window.Integrant.Dominant.GetValue = window.Integrant.Dominant.GetValue || function (ref) {
    return ref.value;
};

window.Integrant.Dominant.SetValue = window.Integrant.Dominant.SetValue || function (ref, value) {
    return ref.value = value;
};

window.Integrant.Dominant.SetDisabled = window.Integrant.Dominant.SetDisabled || function (ref, disabled) {
    return ref.disabled = disabled;
};

window.Integrant.Dominant.SetRequired = window.Integrant.Dominant.SetDisabled || function (ref, required) {
    return ref.required = required;
};

window.Integrant.Dominant.SetPlaceholder = window.Integrant.Dominant.SetPlaceholder || function (ref, placeholder) {
    return ref.placeholder = placeholder;
};
