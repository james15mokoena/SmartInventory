// get the "AddTransactionReason" button.
const btnAddTransactionReason = document.querySelector("#add-btn");
// get the "ViewTransactionReasons" button.
const btnViewTransactionReasons = document.querySelector("#view-btn");

// get the "add-reason-tab"
const addReasonTab = document.querySelector(".add-reason-tab");
// get the "view-reasons-tab"
const viewReasonsTab = document.querySelector(".view-reasons-tab");

/**
 * Responsible for showing or hiding the "add-reason-tab", when the
 * add-btn is clicked.
 */
function showAddReasonTab() {
    // we want to display the add-reason-tab
    if (!addReasonTab.classList.contains("show")) {
        // show the add-reason-tab
        addReasonTab.classList.add("show");
        addReasonTab.classList.remove("hide");
        // hide the view-reasons-tab
        viewReasonsTab.classList.remove("show");
        viewReasonsTab.classList.add("hide");
        // change the colors after selection
        btnAddTransactionReason.style.backgroundColor = "lightseagreen";
        btnViewTransactionReasons.style.backgroundColor = "peru";
    }
}

/**
 * Responsible for showing or hiding the "view-reasons-tab", when the view-btn
 * is clicked.
 */
function showViewReasonsTab() {
    // we want to display the view-reasons-tab
    if (!viewReasonsTab.classList.contains("show")) {
        // show the view-reasons-tab
        viewReasonsTab.classList.add("show");
        viewReasonsTab.classList.remove("hide");
        // hide the add-reason-tab
        addReasonTab.classList.remove("show");
        addReasonTab.classList.add("hide")
        // change the colors after selection
        btnViewTransactionReasons.style.backgroundColor = "lightseagreen";
        btnAddTransactionReason.style.backgroundColor = "peru";
    }
}

// add the event listener
btnAddTransactionReason.addEventListener("click", showAddReasonTab);
btnViewTransactionReasons.addEventListener("click", showViewReasonsTab);