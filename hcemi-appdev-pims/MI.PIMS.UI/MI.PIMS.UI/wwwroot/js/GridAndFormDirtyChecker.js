// GridAndFormDirtyChecker.js
// Utility to track changes in form data and grid data arrays

const GridAndFormDirtyChecker = (function () {
    let formSelectors = [];
    let getGridDataFn = null;
    let originalFormData = '';
    let originalGridDataSnapshot = '';

    /**
     * Initializes the dirty checker with form and grid data sources.
     * @param {Object} options
     * @param {string} options.formId - The selector for the form (e.g., '#editForm').
     * @param {Function} options.getGridData - A function that returns an object of grid data arrays.
     */
    function init({ forms = [], getGridData }) {
        formSelectors = forms;
        getGridDataFn = getGridData;

        originalFormData = getCombinedFormData();
        originalGridDataSnapshot = getGridSnapshot();
    }

    /**
     * Serializes the current form data.
     * @returns {string}
     */
    function getCombinedFormData() {
        return formSelectors.map(sel => $(sel).serialize()).join('&');
    }
    
    /**
     * Serializes the current grid data.
     * @returns {string}
     */
    function getGridSnapshot() {
        if (typeof getGridDataFn === 'function') {
            const gridData = getGridDataFn();
            //console.log(JSON.stringify(gridData));
            return JSON.stringify(gridData);
        }
        return '';
    }

    /**
     * Checks if the form has changed.
     * @returns {boolean}
     */
    function isFormDirty() {
        return getCombinedFormData() !== originalFormData;
    }


    /**
     * Checks if the grid data has changed.
     * @returns {boolean}
     */
    function isGridDirty() {
        return getGridSnapshot() !== originalGridDataSnapshot;
    }

    /**
     * Checks if either the form or grid data has changed.
     * @returns {boolean}
     */
    function isDirty() {
        return isFormDirty() || isGridDirty();
    }

    /**
     * Runs a dirty check and calls the provided toggle function with the result.
     * @param {Function} toggleFn - A function that accepts a boolean (true if dirty).
     */
    function dirtycheck(toggleFn) {
        const dirty = isDirty();
        if (typeof toggleFn === 'function') {
            toggleFn(dirty);
        }
    }

    return {
        init,
        isFormDirty,
        isGridDirty,
        isDirty,
        dirtycheck
    };
})();
