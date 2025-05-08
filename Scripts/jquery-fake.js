// Fake jQuery file to satisfy ScriptResourceMapping
window.jQuery = window.$ = function () {
    console.warn("jquery-fake.js: jQuery stub called.");
    return {
        ready: function (fn) {
            fn();
        }
    };
};
