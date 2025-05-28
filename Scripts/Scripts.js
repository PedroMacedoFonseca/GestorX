import { ModalManager } from './Core/Modals.js';
import './Core/Masks.js';
import './Core/Validators.js';
import './Inicio.js';  

document.addEventListener('DOMContentLoaded', () => {
    if (window.shouldOpenModalCadastro) {
        abrirModalUsuario();
        delete window.shouldOpenModalCadastro;
    }
    if (window.shouldOpenModalUnidade) {
        abrirModalUnidade();
        delete window.shouldOpenModalUnidade;
    }
    if (window.shouldOpenModalGerenciarUnidades) {
        abrirModalGerenciarUnidades();
        delete window.shouldOpenModalGerenciarUnidades;
    }
});

function manterModalUnidadeAberto() {
    $('#modalUnidade').modal('show');
}

function fecharModalUnidade() {
    $('#modalUnidade').modal('hide');
}

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
    var errorPanel = document.getElementById('<%= ((CadastroUnidade)phCadastroUnidade.FindControl("cadUnidade")).pnlValidation.ClientID %>');
    if (errorPanel && errorPanel.style.display !== 'none') {
        $('#modalUnidade').modal('show');
    }
});