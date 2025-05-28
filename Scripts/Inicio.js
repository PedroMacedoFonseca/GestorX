import { ModalManager } from './Core/Modals.js';

window.abrirModalUsuario = () => ModalManager.show('modalCadastro');
window.abrirModalUnidade = () => ModalManager.show('modalUnidade');
window.abrirModalGerenciarUnidades = () => ModalManager.show('modalGerenciarUnidades');

function initializeModals() {
    ModalManager.init('modalCadastro', { keyboard: false, backdrop: 'static' });
    ModalManager.init('modalUnidade');
    ModalManager.init('modalGerenciarUnidades');
}

document.addEventListener('DOMContentLoaded', initializeModals);

if (typeof Sys !== 'undefined') {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        initializeModals();
    });
}