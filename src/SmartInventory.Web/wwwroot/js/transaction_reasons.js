// get the "AddTransactionReason" button.
const btnAddTransactionReason = document.querySelector("#add-btn");
// get the "ViewTransactionReasons" button.
const btnViewTransactionReasons = document.querySelector("#view-btn");

// get the "add-reason-tab"
const addReasonTab = document.querySelector(".add-reason-tab");
// get the "view-reasons-tab"
const viewReasonsTab = document.querySelector(".view-reasons-tab");

/**
 * Displays the tab that is selected by the user.
 * @param {*} tabToShow The tab to be displayed.
 * @param {*} tabToHide The tab to be hidden.
 * @param {*} selectedBtn The button that opens the tab to be displayed.
 * @param {*} deselectedBtn The button that opens the tab to be hidden.
 */
function showOrHideTabHandler(tabToShow, tabToHide, selectedBtn, deselectedBtn) {
    if (!tabToShow.classList.contains("show")) {
        // add the property to show the tab.
        tabToShow.classList.add("show");
        // remove the property for hiding the tab.
        tabToShow.classList.remove("hide");

        // hide the tab that needs to be hidden
        tabToHide.classList.remove("show");
        tabToHide.classList.add("hide");

        // change the color of the selected button and the deselected button
        selectedBtn.style.backgroundColor = "lightseagreen";
        deselectedBtn.style.backgroundColor = "peru";
    }
}

// add the event listener

btnAddTransactionReason.addEventListener("click", () =>
    showOrHideTabHandler(addReasonTab, viewReasonsTab, btnAddTransactionReason, btnViewTransactionReasons));

btnViewTransactionReasons.addEventListener("click", () =>
    showOrHideTabHandler(viewReasonsTab,addReasonTab,btnViewTransactionReasons,btnAddTransactionReason));