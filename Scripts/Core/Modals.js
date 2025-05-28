const ModalManager = {
    instances: {},

    init: function (modalId, options = {}) {
        if (!this.instances[modalId]) {
            const element = document.getElementById(modalId);
            if (element) {
                this.instances[modalId] = new bootstrap.Modal(element, options);
            }
        }
        return this.instances[modalId];
    },

    show: function (modalId) {
        const instance = this.init(modalId);
        if (instance) instance.show();
    },

    hide: function (modalId) {
        if (this.instances[modalId]) {
            this.instances[modalId].hide();
        }
    },

    destroy: function (modalId) {
        if (this.instances[modalId]) {
            this.instances[modalId].dispose();
            delete this.instances[modalId];
            console.log(`ModalManager: Instância '${modalId}' destruída.`);
        }
    }
};

export { ModalManager };
window.ModalManager = ModalManager;