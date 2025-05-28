function applyMasks() {
    if (typeof $.fn.inputmask !== 'function') return;

    $('.cpf-mask').inputmask('999.999.999-99');
    $('.phone-mask').inputmask('(99) [9]9999-9999');
}

function setupMasksForWebForms() {
    applyMasks();

    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            applyMasks();
        });
    }
}

document.addEventListener('DOMContentLoaded', setupMasksForWebForms);