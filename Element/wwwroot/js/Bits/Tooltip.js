window.Integrant = window.Integrant || {};
window.Integrant.Element = window.Integrant.Element || {};

window.Integrant.Element.CreateBitTooltips = window.Integrant.Element.CreateBitTooltips || function () {
    const bits = document.querySelectorAll('[class~="Integrant.Element.Bit"]');

    function createTooltip(v) {
        if (!v.hasOwnProperty('IntegrantElementBitTooltip')) {
            if (v.dataset.hasOwnProperty('integrant.element.bit.tooltip')) {
                v.IntegrantElementBitTooltip = tippy(v, {
                    content: v.dataset['integrant.element.bit.tooltip'],
                });
            }
        } else {
            if (v.dataset.hasOwnProperty('integrant.element.bit.tooltip')) {
                v.IntegrantElementBitTooltip.setContent(v.dataset['integrant.element.bit.tooltip']);
                v.IntegrantElementBitTooltip.enable();
            } else {
                v.IntegrantElementBitTooltip.disable();
            }
        }
    }
    
    const tooltipObserver = new MutationObserver((r) => {
        for (const m of r) {
            if (m.type === 'attributes' && m.attributeName === 'data-integrant.element.bit.tooltip') {
                createTooltip(m.target);
            }
        }
    });
    
    bits.forEach(v => {
        tooltipObserver.observe(v, {
            attributes: true,
            attributeFilter: ['data-integrant.element.bit.tooltip'],
        });

        createTooltip(v);
    });
}
