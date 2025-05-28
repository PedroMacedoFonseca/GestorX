function applyValidators() {
    if (typeof Page_Validators === 'undefined') return;

    Page_Validators.forEach(validator => {
        ValidatorValidate(validator);
    });
}

function setupValidators() {
    applyValidators();

    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            applyValidators();
        });
    }
}