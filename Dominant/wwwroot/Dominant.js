window.Integrant = window.Integrant || {};
window.Integrant.Dominant = window.Integrant.Dominant || {};

window.Integrant.Dominant.GetValue = window.Integrant.Dominant.GetValue || function(ref) {
    return ref.value;
};

window.Integrant.Dominant.SetValue = window.Integrant.Dominant.SetValue || function(ref, value) {
    return ref.value = value;
};
