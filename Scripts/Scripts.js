import { ModalManager } from './Core/Modals.js';
import './Core/Masks.js';      
import './Core/Validators.js'; 
import { setupGlobalModalActions, initializePageModals } from './Inicio.js';

function checkAndOpenModalsFromFlags() {
    if (window.shouldOpenModalCadastro) {
        ModalManager.show('modalCadastro'); 
        delete window.shouldOpenModalCadastro;
    }
    if (window.shouldOpenModalUnidade) {
        ModalManager.show('modalUnidade');
        delete window.shouldOpenModalUnidade;
    }
    if (window.shouldOpenModalGerenciarUnidades) {
        ModalManager.show('modalGerenciarUnidades');
        delete window.shouldOpenModalGerenciarUnidades;
    }
}

function onPageReady() {
    initializePageModals();          
    checkAndOpenModalsFromFlags(); 

    if (window.reopenModalUnidadeOnError) { 
        ModalManager.show('modalUnidade');
        delete window.reopenModalUnidadeOnError;
    }
}

document.addEventListener('DOMContentLoaded', () => {;
    setupGlobalModalActions(); 
    onPageReady();             
});

if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded((sender, args) => {
        onPageReady();
    });
} 