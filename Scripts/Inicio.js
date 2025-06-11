import { ModalManager } from './Core/Modals.js';

export function setupGlobalModalActions() {
    window.abrirModalUsuario = () => ModalManager.show('modalCadastro');
    window.abrirModalUnidade = () => ModalManager.show('modalUnidade');
    window.abrirModalGerenciarUnidades = () => ModalManager.show('modalGerenciarUnidades');

    window.manterModalUnidadeAbertoGlobal = () => ModalManager.show('modalUnidade');
    window.fecharModalUnidadeGlobal = () => ModalManager.hide('modalUnidade');
}

export function initializePageModals() {
    ModalManager.init('modalCadastro', { keyboard: false, backdrop: 'static' });
    ModalManager.init('modalUnidade');
    ModalManager.init('modalGerenciarUnidades');
}