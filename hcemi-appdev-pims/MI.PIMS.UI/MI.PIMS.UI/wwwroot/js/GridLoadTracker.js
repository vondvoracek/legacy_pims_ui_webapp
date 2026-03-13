// GridLoadTracker.js
// Utility to track when multiple Kendo UI grids have finished loading

const GridLoadTracker = (function () {
    let state = {};
    let onAllLoadedCallback = null;
    let formData = '';

    /**
     * Marks a grid as loaded.
     * @param {string} gridName - The unique name of the grid.
     */
    function markLoaded(gridName) {
        state[gridName] = true;
        checkAllLoaded();
    }

    /**
     * Registers a callback to be executed when all tracked grids are loaded.
     * @param {Function} callback - The function to call once all grids are loaded.
     */
    function onAllLoaded(callback) {
        onAllLoadedCallback = callback;
        checkAllLoaded(); // In case all were already marked loaded
    }

    /**
     * Checks if all tracked grids are loaded and triggers the callback if so.
     */
    function checkAllLoaded() {
        const allLoaded = Object.values(state).length > 3 && Object.values(state).every(Boolean);
        if (allLoaded && typeof onAllLoadedCallback === 'function') {
            onAllLoadedCallback();
            onAllLoadedCallback = null; // Prevent future calls
        }
    }

    function setFormData(data) {
        formData = data;
    }

    function getFormData() {
        return formData;
    }


    function reset() {
        state = {};
        onAllLoadedCallback = null;
    }

    return {
        markLoaded,
        onAllLoaded,
        setFormData,
        getFormData,
        reset
    };

})();
